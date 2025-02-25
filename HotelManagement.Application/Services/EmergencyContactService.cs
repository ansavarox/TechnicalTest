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
    /// Provides services for managing emergency contacts associated with reservations.
    /// </summary>
    public class EmergencyContactService
    {
        private readonly IEmergencyContactRepository _contactRepository;
        private readonly IReservationRepository _reservationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmergencyContactService"/> class.
        /// </summary>
        /// <param name="contactRepository">The repository for managing emergency contacts.</param>
        /// <param name="reservationRepository">The repository for managing reservations.</param>
        public EmergencyContactService(
            IEmergencyContactRepository contactRepository,
            IReservationRepository reservationRepository)
        {
            _contactRepository = contactRepository;
            _reservationRepository = reservationRepository;
        }

        /// <summary>
        /// Retrieves the emergency contact details for a given reservation.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation.</param>
        /// <returns>
        /// An <see cref="EmergencyContactDto"/> containing the contact details if found;
        /// otherwise, <c>null</c>.
        /// </returns>
        public async Task<EmergencyContactDto?> GetByReservationIdAsync(int reservationId)
        {
            var contact = await _contactRepository.GetByReservationIdAsync(reservationId);

            if (contact == null) return null;

            return new EmergencyContactDto
            {
                FullName = contact.Fullname,
                Phone = contact.Phone
            };
        }

        /// <summary>
        /// Adds a new emergency contact or updates an existing one for a given reservation.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation.</param>
        /// <param name="contactDto">The contact details to add or update.</param>
        /// <returns>
        /// <c>true</c> if the contact was added or updated successfully; otherwise, <c>false</c> if the reservation does not exist.
        /// </returns>
        public async Task<bool> AddOrUpdateAsync(int reservationId, EmergencyContactDto contactDto)
        {
            var reservationExists = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservationExists == null)
            {
                return false;
            }

            var existingContact = await _contactRepository.GetByReservationIdAsync(reservationId);

            if (existingContact != null)
            {
                existingContact.Fullname = contactDto.FullName;
                existingContact.Phone = contactDto.Phone;
                await _contactRepository.UpdateAsync(existingContact);
            }
            else
            {
                var newContact = new Emergencycontact
                {
                    Reservationid = reservationId,
                    Fullname = contactDto.FullName,
                    Phone = contactDto.Phone
                };
                await _contactRepository.AddAsync(newContact);
            }

            return true;
        }
    }
}
