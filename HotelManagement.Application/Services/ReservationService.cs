using HotelManagement.Application.DTOs;
using HotelManagement.Application.Interfaces;
using HotelManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.Services
{
    /// <summary>
    /// Service for handling hotel reservations.
    /// </summary>
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomRepository _roomRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationService"/> class.
        /// </summary>
        /// <param name="reservationRepository">The repository responsible for reservation data access.</param>
        /// <param name="hotelRepository">The repository responsible for hotel data access.</param>
        /// <param name="roomRepository">The repository responsible for room data access.</param>
        public ReservationService(
            IReservationRepository reservationRepository,
            IHotelRepository hotelRepository,
            IRoomRepository roomRepository)
        {
            _reservationRepository = reservationRepository;
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// Retrieves all reservations for a given hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <returns>A list of reservations for the specified hotel.</returns>
        public async Task<IEnumerable<ReservationDto>> GetReservationsByHotelAsync(int hotelId)
        {
            var reservations = await _reservationRepository.GetByHotelIdAsync(hotelId);

            return reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                HotelId = r.Hotelid,
                RoomId = r.Roomid,
                TravelerId = r.Travelerid,
                CheckInDate = r.Checkindate.ToDateTime(TimeOnly.MinValue),
                CheckOutDate = r.Checkoutdate.ToDateTime(TimeOnly.MinValue),
                TotalCost = r.Totalcost
            });
        }

        /// <summary>
        /// Retrieves detailed information of a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation.</param>
        /// <returns>The reservation details.</returns>
        public async Task<ReservationDetailDto> GetReservationByIdAsync(int id)
        {
            return await _reservationRepository.GetReservationByIdAsync(id);
        }

        /// <summary>
        /// Retrieves a simplified version of a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation.</param>
        /// <returns>A simplified reservation detail response, or null if not found.</returns>
        public async Task<ReservationDetailResponseDto?> GetSimpleReservationByIdAsync(int id)
        {
            return await _reservationRepository.GetSimpleReservationByIdAsync(id);
        }

        /// <summary>
        /// Checks if a specific room is available for the given date range.
        /// </summary>
        /// <param name="roomId">The ID of the room.</param>
        /// <param name="checkIn">The check-in date.</param>
        /// <param name="checkOut">The check-out date.</param>
        /// <returns>True if the room is available; otherwise, false.</returns>
        public async Task<bool> IsRoomAvailable(int roomId, DateOnly checkIn, DateOnly checkOut)
        {
            return await _reservationRepository.IsRoomAvailable(roomId, checkIn, checkOut);
        }

        /// <summary>
        /// Creates a new reservation for a user in a specified hotel and room.
        /// </summary>
        /// <param name="userId">The ID of the traveler making the reservation.</param>
        /// <param name="hotelId">The ID of the hotel where the reservation is made.</param>
        /// <param name="roomId">The ID of the room being reserved.</param>
        /// <param name="checkIn">The check-in date.</param>
        /// <param name="checkOut">The check-out date.</param>
        /// <returns>The created reservation object.</returns>
        public async Task<Reservation> CreateReservation(int userId, int hotelId, int roomId, DateOnly checkIn, DateOnly checkOut)
        {
            
            if (checkIn >= checkOut)
            {
                throw new ArgumentException("Check-in date must be before check-out date.");
            }

          
            if (checkIn < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new ArgumentException("Check-in date cannot be in the past.");
            }

           
            var hotelExists = await _hotelRepository.ExistsAsync(hotelId);
            if (!hotelExists)
            {
                throw new ArgumentException("The specified hotel does not exist.");
            }

            var roomExists = await _roomRepository.ExistsAsync(roomId);
            if (!roomExists)
            {
                throw new ArgumentException("The specified room does not exist.");
            }

            bool isRoomAvailable = await _reservationRepository.IsRoomAvailable(roomId, checkIn, checkOut);
            if (!isRoomAvailable)
            {
                throw new ArgumentException("The room is already booked for the selected dates.");
            }

            var reservation = new Reservation
            {
                Travelerid = userId,
                Hotelid = hotelId,
                Roomid = roomId,
                Checkindate = checkIn,
                Checkoutdate = checkOut
            };

            return await _reservationRepository.AddReservation(reservation);
        }
    }
}
