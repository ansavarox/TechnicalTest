using HotelManagement.Application.DTOs;
using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.Interfaces
{
    /// <summary>
    /// Defines methods for managing hotel data in the repository.
    /// </summary>
    public interface IHotelRepository
    {
        /// <summary>
        /// Retrieves all hotels from the database.
        /// </summary>
        /// <returns>A collection of hotels.</returns>
        Task<IEnumerable<Hotel>> GetAllAsync();

        /// <summary>
        /// Retrieves a specific hotel by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the hotel.</param>
        /// <returns>The hotel if found, otherwise null.</returns>
        Task<Hotel?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new hotel to the database.
        /// </summary>
        /// <param name="hotel">The hotel entity to be added.</param>
        Task AddAsync(Hotel hotel);

        /// <summary>
        /// Updates an existing hotel in the database.
        /// </summary>
        /// <param name="hotel">The hotel entity with updated information.</param>
        Task UpdateAsync(Hotel hotel);

        /// <summary>
        /// Adds multiple hotels to the database in a single operation.
        /// </summary>
        /// <param name="hotels">A collection of hotels to be added.</param>
        Task AddRangeAsync(IEnumerable<Hotel> hotels);
    }
}
