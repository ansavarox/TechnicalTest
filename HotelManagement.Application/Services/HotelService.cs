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
    /// Provides services for managing hotel-related operations.
    /// </summary>
    public class HotelService
    {
        private readonly IHotelRepository _hotelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotelService"/> class.
        /// </summary>
        /// <param name="hotelRepository">The repository for managing hotels.</param>
        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        /// <summary>
        /// Retrieves a list of all hotels.
        /// </summary>
        /// <returns>A collection of <see cref="HotelDto"/> objects representing the hotels.</returns>
        public async Task<IEnumerable<HotelDto>> GetHotelsAsync()
        {
            var hotels = await _hotelRepository.GetAllAsync();
            return hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Location = h.Location,
                IsActive = (bool)h.Isactive
            });
        }

        /// <summary>
        /// Retrieves details of a specific hotel by its ID.
        /// </summary>
        /// <param name="id">The ID of the hotel.</param>
        /// <returns>
        /// A <see cref="HotelDto"/> containing hotel details if found.
        /// </returns>
        public async Task<HotelDto> GetHotelByIdAsync(int id)
        {
            var h = await _hotelRepository.GetByIdAsync(id);
            HotelDto hotelDto = new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Location = h.Location,
                IsActive = (bool)h.Isactive
            };
            return hotelDto;
        }

        /// <summary>
        /// Adds a new hotel to the system.
        /// </summary>
        /// <param name="hotel">The hotel entity to add.</param>
        public async Task AddHotelAsync(Hotel hotel)
        {
            await _hotelRepository.AddAsync(hotel);
        }

        /// <summary>
        /// Updates an existing hotel based on its ID.
        /// </summary>
        /// <param name="id">The ID of the hotel to update.</param>
        /// <param name="hotelDto">The DTO containing updated hotel information.</param>
        /// <returns><c>true</c> if the hotel was updated successfully; otherwise, <c>false</c>.</returns>
        public async Task<bool> UpdateHotelAsync(int id, HotelUpdateDto hotelDto)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null)
                return false;

            if (!string.IsNullOrEmpty(hotelDto.Name))
                hotel.Name = hotelDto.Name;

            if (!string.IsNullOrEmpty(hotelDto.Location))
                hotel.Location = hotelDto.Location;

            if (hotelDto.IsActive.HasValue)
                hotel.Isactive = hotelDto.IsActive.Value;

            await _hotelRepository.UpdateAsync(hotel);
            return true;
        }

        /// <summary>
        /// Updates an existing hotel`s status based on its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public async Task<bool> UpdateHotelStatusAsync(int id, bool isActive)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null)
                return false;

            hotel.Isactive = isActive;
            await _hotelRepository.UpdateAsync(hotel);
            return true;
        }

        /// <summary>
        /// Adds multiple hotels to the system in bulk.
        /// </summary>
        /// <param name="hotels">A collection of hotels to add.</param>
        public async Task AddRangeAsync(IEnumerable<Hotel> hotels)
        {
            await _hotelRepository.AddRangeAsync(hotels);
        }
    }
}
