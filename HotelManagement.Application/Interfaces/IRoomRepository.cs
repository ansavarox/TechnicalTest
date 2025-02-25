using HotelManagement.Application.DTOs;
using HotelManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.Interfaces
{
    /// <summary>
    /// Defines methods for managing hotel rooms.
    /// </summary>
    public interface IRoomRepository
    {
        /// <summary>
        /// Retrieves a room by its unique identifier.
        /// </summary>
        /// <param name="roomId">The unique ID of the room.</param>
        /// <returns>The room entity if found, otherwise null.</returns>
        Task<Room?> GetByIdAsync(int roomId);

        /// <summary>
        /// Retrieves a list of basic room details for a specific hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <returns>A collection of room details associated with the hotel.</returns>
        Task<IEnumerable<RoomBasicDto>> GetRoomsByHotelIdAsync(int hotelId);

        /// <summary>
        /// Adds a new room to the database.
        /// </summary>
        /// <param name="room">The room entity to be added.</param>
        Task AddAsync(Room room);

        /// <summary>
        /// Updates an existing room's details.
        /// </summary>
        /// <param name="room">The room entity with updated information.</param>
        Task UpdateAsync(Room room);

        /// <summary>
        /// Adds multiple rooms to the database in a batch operation.
        /// </summary>
        /// <param name="rooms">A collection of rooms to be added.</param>
        Task AddRangeAsync(IEnumerable<Room> rooms);

        /// <summary>
        /// Searches for available rooms based on check-in and check-out dates, number of guests, and city.
        /// </summary>
        /// <param name="checkIn">The check-in date.</param>
        /// <param name="checkOut">The check-out date.</param>
        /// <param name="guests">The number of guests.</param>
        /// <param name="city">The city where the rooms are being searched.</param>
        /// <returns>A list of available rooms matching the search criteria.</returns>
        Task<IEnumerable<RoomBasicDto>> SearchRoomsAsync(DateTime checkIn, DateTime checkOut, int guests, string city);

        /// <summary>
        /// Retrieves the maximum guest capacity for a reservation.
        /// </summary>
        /// <param name="reservationId">The reservation ID.</param>
        /// <returns>The total guest capacity of the reserved room.</returns>
        Task<int> GetRoomCapacityByReservationIdAsync(int reservationId);
    }
}
