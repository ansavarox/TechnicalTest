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
    /// Defines methods for managing hotel reservations.
    /// </summary>
    public interface IReservationRepository
    {
        /// <summary>
        /// Retrieves all reservations associated with a specific hotel.
        /// </summary>
        /// <param name="id">The unique identifier of the hotel.</param>
        /// <returns>A collection of reservations for the specified hotel.</returns>
        Task<IEnumerable<Reservation>> GetByHotelIdAsync(int id);

        /// <summary>
        /// Retrieves a reservation by its unique identifier.
        /// </summary>
        /// <param name="id">The reservation ID.</param>
        /// <returns>A DTO containing basic reservation details, or null if not found.</returns>
        Task<ReservationDto?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves detailed information about a reservation.
        /// </summary>
        /// <param name="id">The reservation ID.</param>
        /// <returns>A DTO containing detailed reservation information.</returns>
        Task<ReservationDetailDto> GetReservationByIdAsync(int id);

        /// <summary>
        /// Retrieves simplified reservation details.
        /// </summary>
        /// <param name="id">The reservation ID.</param>
        /// <returns>A DTO with basic reservation details, or null if not found.</returns>
        Task<ReservationDetailResponseDto?> GetSimpleReservationByIdAsync(int id);

        /// <summary>
        /// Checks if a specific room is available for a given date range.
        /// </summary>
        /// <param name="roomId">The room ID to check.</param>
        /// <param name="checkIn">The check-in date.</param>
        /// <param name="checkOut">The check-out date.</param>
        /// <returns>True if the room is available, otherwise false.</returns>
        Task<bool> IsRoomAvailable(int roomId, DateOnly checkIn, DateOnly checkOut);

        /// <summary>
        /// Adds a new reservation to the system.
        /// </summary>
        /// <param name="reservation">The reservation entity to be added.</param>
        /// <returns>The created reservation entity.</returns>
        Task<Reservation> AddReservation(Reservation reservation);
    }
}