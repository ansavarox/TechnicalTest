using HotelManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Application.Interfaces;
using HotelManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Application.DTOs;

namespace HotelManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing hotel data.
    /// </summary>
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotelRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public HotelRepository(HotelDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all hotels from the database.
        /// </summary>
        /// <returns>A list of all hotels.</returns>
        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _context.Hotels.ToListAsync();
        }

        /// <summary>
        /// Retrieves a hotel by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the hotel.</param>
        /// <returns>The hotel if found; otherwise, null.</returns>
        public async Task<Hotel?> GetByIdAsync(int id)
        {
            return await _context.Hotels.FindAsync(id);
        }

        /// <summary>
        /// Adds a new hotel to the database.
        /// </summary>
        /// <param name="hotel">The hotel entity to add.</param>
        public async Task AddAsync(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing hotel in the database.
        /// </summary>
        /// <param name="hotel">The hotel entity to update.</param>
        public async Task UpdateAsync(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing hotel`s status based on its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public async Task<bool> UpdateHotelStatusAsync(int id, bool isActive)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
                return false;

            hotel.Isactive = isActive;
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Adds multiple hotels to the database in a single transaction.
        /// </summary>
        /// <param name="hotels">A collection of hotel entities to add.</param>
        public async Task AddRangeAsync(IEnumerable<Hotel> hotels)
        {
            await _context.Hotels.AddRangeAsync(hotels);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Validates if hotel exists
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(int hotelId)
        {
            return await _context.Hotels.AnyAsync(h => h.Id == hotelId);
        }
    }
}
