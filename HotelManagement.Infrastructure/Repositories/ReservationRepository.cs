using HotelManagement.Application.DTOs;
using HotelManagement.Application.Interfaces;
using HotelManagement.Domain.Entities;
using HotelManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing reservations.
    /// </summary>
    public class ReservationRepository : IReservationRepository
    {
        private readonly HotelDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ReservationRepository(HotelDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all reservations for a specific hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        public async Task<IEnumerable<Reservation>> GetByHotelIdAsync(int hotelId)
        {
            return await _context.Reservations
                .Where(r => r.Hotelid == hotelId)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a reservation by its ID and maps it to a DTO.
        /// </summary>
        /// <param name="id">The reservation ID.</param>
        /// <returns>The reservation DTO if found; otherwise, null.</returns>
        public async Task<ReservationDto?> GetByIdAsync(int id)
        {
            var reservation = await _context.Reservations
                .Where(r => r.Id == id)
                .Select(r => new ReservationDto
                {
                    Id = r.Id,
                    HotelId = r.Hotelid,
                    RoomId = r.Roomid,
                    TravelerId = r.Travelerid,
                    CheckInDate = r.Checkindate.ToDateTime(TimeOnly.MinValue),
                    CheckOutDate = r.Checkoutdate.ToDateTime(TimeOnly.MinValue),
                    TotalCost = r.Totalcost
                })
                .FirstOrDefaultAsync();

            return reservation;
        }

        /// <summary>
        /// Retrieves detailed reservation information by ID, including hotel, room, traveler, guests, and emergency contacts.
        /// </summary>
        /// <param name="id">The reservation ID.</param>
        /// <returns>The reservation detail DTO if found; otherwise, null.</returns>
        public async Task<ReservationDetailDto?> GetReservationByIdAsync(int id)
        {
            var reservation = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.Hotel)
                .ThenInclude(h => h.Rooms)
                .Include(r => r.Room)
                .Include(r => r.Traveler)
                .Include(r => r.Reservationguests)
                .Include(r => r.Emergencycontacts)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return null;

            return new ReservationDetailDto
            {
                Id = reservation.Id,
                HotelName = reservation.Hotel.Name,
                RoomType = reservation.Room.RoomType,
                RoomId = reservation.Roomid,
                TravelerName = reservation.Traveler.Fullname,
                CheckInDate = reservation.Checkindate,
                CheckOutDate = reservation.Checkoutdate,
                TotalCost = reservation.Totalcost,
                Guests = reservation.Reservationguests.Select(g => new ReservationGuestDto
                {
                    FullName = g.Fullname,
                    BirthDate = g.Birthdate,
                    Gender = g.Gender,
                    DocumentType = g.Documenttype,
                    DocumentNumber = g.Documentnumber,
                    Email = g.Email,
                    Phone = g.Phone
                }).ToList(),
                EmergencyContacts = reservation.Emergencycontacts.Select(c => new EmergencyContactDto
                {
                    FullName = c.Fullname,
                    Phone = c.Phone
                }).ToList()
            };
        }

        /// <summary>
        /// Retrieves a simplified reservation DTO by ID, including basic details such as hotel, room, and traveler.
        /// </summary>
        /// <param name="id">The reservation ID.</param>
        /// <returns>The simple reservation DTO if found; otherwise, null.</returns>
        public async Task<ReservationDetailResponseDto?> GetSimpleReservationByIdAsync(int id)
        {
            var reservation = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.Hotel)
                .Include(r => r.Room)
                .Include(r => r.Traveler)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return null;

            return new ReservationDetailResponseDto
            {
                Id = reservation.Id,
                HotelId = reservation.Hotelid,
                HotelName = reservation.Hotel?.Name ?? "Unknown Hotel",
                RoomId = reservation.Roomid,
                RoomType = reservation.Room?.RoomType ?? "Unknown Room",
                TravelerId = reservation.Travelerid,
                TravelerName = reservation.Traveler?.Fullname ?? "Unknown Traveler",
                CheckInDate = reservation.Checkindate,
                CheckOutDate = reservation.Checkoutdate,
                TotalCost = reservation.Totalcost,
                CreatedAt = reservation.Createdat
            };
        }

        /// <summary>
        /// Checks whether a room is available for a given date range.
        /// </summary>
        /// <param name="roomId">The ID of the room.</param>
        /// <param name="checkIn">The check-in date.</param>
        /// <param name="checkOut">The check-out date.</param>
        /// <returns>True if the room is available; otherwise, false.</returns>
        public async Task<bool> IsRoomAvailable(int roomId, DateOnly checkIn, DateOnly checkOut)
        {
            return !await _context.Reservations.AnyAsync(r =>
                r.Roomid == roomId &&
                ((r.Checkindate < checkOut && r.Checkoutdate > checkIn) || (r.Checkindate == checkIn))
            );
        }

        /// <summary>
        /// Adds a new reservation to the database, calculating the total cost based on the room's base cost and taxes.
        /// </summary>
        /// <param name="reservation">The reservation entity to add.</param>
        /// <returns>The added reservation.</returns>
        public async Task<Reservation> AddReservation(Reservation reservation)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == reservation.Roomid);
            if (room == null)
            {
                throw new Exception("Room not found.");
            }

            int nights = (int)(reservation.Checkoutdate.ToDateTime(TimeOnly.MinValue) -
                                reservation.Checkindate.ToDateTime(TimeOnly.MinValue)).TotalDays;

            reservation.Totalcost = nights * room.Basecost + room.Taxes;
            reservation.Createdat = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);


            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }
    }
}
