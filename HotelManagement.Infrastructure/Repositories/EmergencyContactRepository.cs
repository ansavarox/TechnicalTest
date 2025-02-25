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
    /// Repository for managing emergency contact data.
    /// </summary>
    public class EmergencyContactRepository : IEmergencyContactRepository
    {
        private readonly HotelDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmergencyContactRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public EmergencyContactRepository(HotelDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves an emergency contact associated with a specific reservation.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation.</param>
        /// <returns>The emergency contact if found; otherwise, null.</returns>
        public async Task<Emergencycontact?> GetByReservationIdAsync(int reservationId)
        {
            return await _context.Emergencycontacts
                .FirstOrDefaultAsync(ec => ec.Reservationid == reservationId);
        }

        /// <summary>
        /// Adds a new emergency contact to the database.
        /// </summary>
        /// <param name="contact">The emergency contact entity to add.</param>
        public async Task AddAsync(Emergencycontact contact)
        {
            await _context.Emergencycontacts.AddAsync(contact);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing emergency contact in the database.
        /// </summary>
        /// <param name="contact">The emergency contact entity to update.</param>
        public async Task UpdateAsync(Emergencycontact contact)
        {
            _context.Emergencycontacts.Update(contact);
            await _context.SaveChangesAsync();
        }
    }
}
