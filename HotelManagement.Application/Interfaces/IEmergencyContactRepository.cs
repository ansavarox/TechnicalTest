using HotelManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.Interfaces
{
    /// <summary>
    /// Defines methods for managing emergency contacts associated with reservations.
    /// </summary>
    public interface IEmergencyContactRepository
    {
        /// <summary>
        /// Retrieves the emergency contact associated with a specific reservation.
        /// </summary>
        /// <param name="reservationId">The unique identifier of the reservation.</param>
        /// <returns>The emergency contact if found, otherwise null.</returns>
        Task<Emergencycontact?> GetByReservationIdAsync(int reservationId);

        /// <summary>
        /// Adds a new emergency contact to the database.
        /// </summary>
        /// <param name="contact">The emergency contact to be added.</param>
        Task AddAsync(Emergencycontact contact);

        /// <summary>
        /// Updates an existing emergency contact in the database.
        /// </summary>
        /// <param name="contact">The emergency contact with updated information.</param>
        Task UpdateAsync(Emergencycontact contact);
    }
}
