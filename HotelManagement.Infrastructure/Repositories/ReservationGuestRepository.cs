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
    /// Repository for managing reservation guests.
    /// </summary>
    public class ReservationGuestRepository : IReservationGuestRepository
    {
        private readonly HotelDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationGuestRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ReservationGuestRepository(HotelDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the total number of guests for a given reservation.
        /// </summary>
        /// <param name="reservationId">The reservation ID.</param>
        /// <returns>The count of guests associated with the reservation.</returns>
        public async Task<int> GetGuestCountByReservationIdAsync(int reservationId)
        {
            return await _context.Reservationguests
            .AsQueryable()
            .CountAsync(g => g.Reservationid == reservationId)
            .ConfigureAwait(false);
        }

        /// <summary>
        /// Adds a single guest to the reservation.
        /// </summary>
        /// <param name="guest">The guest entity to add.</param>
        /// <returns>True if the guest was added successfully; otherwise, false.</returns>
        public async Task<bool> AddGuestAsync(Reservationguest guest)
        {
            try
            {
                await _context.Reservationguests.AddAsync(guest);
                return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the guest.", ex);
            }
        }

        /// <summary>
        /// Adds multiple guests to a reservation.
        /// </summary>
        /// <param name="guests">The list of guests to add.</param>
        /// <returns>True if guests were added successfully; otherwise, false.</returns>
        public async Task<bool> AddGuestsAsync(IEnumerable<Reservationguest> guests)
        {
            await _context.Reservationguests.AddRangeAsync(guests);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
