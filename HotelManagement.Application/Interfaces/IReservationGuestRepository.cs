using HotelManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.Interfaces
{
    /// <summary>
    /// Defines methods for managing guests associated with reservations.
    /// </summary>
    public interface IReservationGuestRepository
    {
        /// <summary>
        /// Retrieves the number of guests for a specific reservation.
        /// </summary>
        /// <param name="reservationId">The unique identifier of the reservation.</param>
        /// <returns>The total number of guests associated with the reservation.</returns>
        Task<int> GetGuestCountByReservationIdAsync(int reservationId);

        /// <summary>
        /// Adds a single guest to a reservation.
        /// </summary>
        /// <param name="guest">The guest entity to be added.</param>
        /// <returns>True if the guest was successfully added, otherwise false.</returns>
        Task<bool> AddGuestAsync(Reservationguest guest);

        /// <summary>
        /// Adds multiple guests to a reservation.
        /// </summary>
        /// <param name="guests">A collection of guest entities to be added.</param>
        /// <returns>True if the guests were successfully added, otherwise false.</returns>
        Task<bool> AddGuestsAsync(IEnumerable<Reservationguest> guests);
    }
}
