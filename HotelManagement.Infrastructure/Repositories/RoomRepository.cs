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
    /// Repository for managing rooms data.
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public RoomRepository(HotelDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of rooms for a given hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <returns>A list of rooms belonging to the specified hotel.</returns>
        public async Task<IEnumerable<RoomBasicDto>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _context.Rooms
                .Where(r => r.Hotelid == hotelId)
                .Select(r => new RoomBasicDto
                {
                    Id = r.Id,
                    HotelId = r.Hotelid,
                    Capacity = r.Capacity,
                    BaseCost = r.Basecost,
                    Taxes = r.Taxes,
                    IsActive = r.Isactive ?? false,
                    Location = r.Location,
                    RoomType = r.RoomType
                })
                .ToListAsync();
        }

        /// <summary>
        /// Adds a single room to the database.
        /// </summary>
        /// <param name="room">The room entity to add.</param>
        public async Task AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds multiple rooms to the database.
        /// </summary>
        /// <param name="rooms">A collection of rooms to add.</param>
        public async Task AddRangeAsync(IEnumerable<Room> rooms)
        {
            await _context.Rooms.AddRangeAsync(rooms);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a room by its ID.
        /// </summary>
        /// <param name="roomId">The ID of the room.</param>
        /// <returns>The room entity if found; otherwise, null.</returns>
        public async Task<Room?> GetByIdAsync(int roomId)
        {
            return await _context.Rooms.FindAsync(roomId);
        }

        /// <summary>
        /// Updates an existing room in the database.
        /// </summary>
        /// <param name="room">The room entity with updated values.</param>
        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Searches for available rooms based on check-in and check-out dates, number of guests, and city.
        /// </summary>
        /// <param name="checkIn">Check-in date.</param>
        /// <param name="checkOut">Check-out date.</param>
        /// <param name="guests">Number of guests.</param>
        /// <param name="city">City where the hotel is located.</param>
        /// <returns>A list of available rooms that match the criteria.</returns>
        public async Task<IEnumerable<RoomBasicDto>> SearchRoomsAsync(DateTime checkIn, DateTime checkOut, int guests, string city)
        {
            var checkInDateTime = new DateTime(checkIn.Year, checkIn.Month, checkIn.Day);
            var checkOutDateTime = new DateTime(checkOut.Year, checkOut.Month, checkOut.Day);

            var rooms = await _context.Rooms
                .Where(r => r.Hotel.Location == city && r.Capacity >= guests &&
                    !_context.Reservations.Any(res =>
                        res.Roomid == r.Id &&
                        new DateTime(res.Checkindate.Year, res.Checkindate.Month, res.Checkindate.Day) < checkOutDateTime &&
                        new DateTime(res.Checkoutdate.Year, res.Checkoutdate.Month, res.Checkoutdate.Day) > checkInDateTime))
                .Select(r => new RoomBasicDto
                {
                    Id = r.Id,
                    HotelId = r.Hotelid,
                    Capacity = r.Capacity,
                    BaseCost = r.Basecost,
                    Taxes = r.Taxes,
                    IsActive = r.Isactive ?? false,
                    Location = r.Location,
                    RoomType = r.RoomType
                })
                .ToListAsync();

            return rooms;
        }

        /// <summary>
        /// Retrieves the capacity of a room based on a reservation ID.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation.</param>
        /// <returns>The capacity of the associated room.</returns>
        public async Task<int> GetRoomCapacityByReservationIdAsync(int reservationId)
        {
            var room = await _context.Reservations
                .Where(r => r.Id == reservationId)
                .Select(r => r.Room)
                .FirstOrDefaultAsync();

            return room?.Capacity ?? 0;
        }
    }
}