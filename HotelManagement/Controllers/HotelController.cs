using HotelManagement.Application.DTOs;
using HotelManagement.Application.Services;
using HotelManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagement.Api.Controllers
{
    /// <summary>
    /// Manages hotel-related operations.
    /// </summary>
    [Authorize(Roles = "Agent")]
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly HotelService _hotelService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotelController"/> class.
        /// </summary>
        /// <param name="hotelService">The hotel service.</param>
        public HotelController(HotelService hotelService)
        {
            _hotelService = hotelService;
        }

        /// <summary>
        /// Retrieves a list of all hotels.
        /// </summary>
        /// <returns>A list of hotels.</returns>
        /// <response code="200">Returns the list of hotels.</response>
        [HttpGet("GetAllHotels")]
        [ProducesResponseType(typeof(IEnumerable<HotelDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            return Ok(await _hotelService.GetHotelsAsync());
        }

        /// <summary>
        /// Retrieves a specific hotel by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the hotel.</param>
        /// <returns>The hotel details if found.</returns>
        /// <response code="200">Returns the hotel details.</response>
        /// <response code="404">If the hotel is not found.</response>
        [HttpGet("GetHotelById")]
        [ProducesResponseType(typeof(HotelDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null) return NotFound();
            return Ok(hotel);
        }

        /// <summary>
        /// Creates multiple hotels.
        /// </summary>
        /// <param name="hotelsDto">A list of hotels to be created.</param>
        /// <returns>A response indicating the creation status.</returns>
        /// <response code="201">Indicates that the hotels were created successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        [HttpPost("CreateHotels")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateHotels([FromBody] List<HotelCreateDto> hotelsDto)
        {
            if (hotelsDto == null || !hotelsDto.Any())
                return BadRequest(new { message = "Hotel data is required." });

            var hotels = hotelsDto.Select(h => new Hotel
            {
                Name = h.Name,
                Location = h.Location,
                Isactive = h.IsActive
            }).ToList();

            await _hotelService.AddRangeAsync(hotels);

            return Created();
        }


        /// <summary>
        /// Updates an existing hotel by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the hotel.</param>
        /// <param name="hotelDto">The updated hotel data.</param>
        /// <returns>A response indicating the update status.</returns>
        /// <response code="200">Indicates that the hotel was successfully updated.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="404">If the hotel is not found.</response>
        [HttpPatch("UpdateHotelById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelUpdateDto hotelDto)
        {
            if (hotelDto == null)
                return BadRequest(new { message = "Invalid request body." });

            var updated = await _hotelService.UpdateHotelAsync(id, hotelDto); // ✅ Ahora pasamos el id

            if (!updated)
                return NotFound(new { message = "Hotel not found." });

            return Ok(new { message = "Hotel updated successfully." });
        }
    }
}

