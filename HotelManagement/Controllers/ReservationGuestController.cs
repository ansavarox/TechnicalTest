using HotelManagement.Application.DTOs;
using HotelManagement.Application.Services;
using HotelManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.API.Controllers
{
    /// <summary>
    /// Handles operations related to guests associated with reservations.
    /// </summary>
    [Authorize(Roles = "Traveler")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationGuestController : ControllerBase
    {
        private readonly ReservationGuestService _reservationGuestService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationGuestController"/> class.
        /// </summary>
        /// <param name="reservationGuestService">The reservation guest service.</param>
        public ReservationGuestController(ReservationGuestService reservationGuestService)
        {
            _reservationGuestService = reservationGuestService;
        }

        /// <summary>
        /// Adds guests to a specific reservation.
        /// </summary>
        /// <param name="reservationId">The unique identifier of the reservation.</param>
        /// <param name="guestsDto">The list of guests to be added.</param>
        /// <returns>A response indicating whether the guests were successfully added.</returns>
        /// <response code="201">Guests were successfully added.</response>
        /// <response code="400">If the request data is invalid or guests could not be added due to validation errors.</response>
        [HttpPost("CreateGuestsByReservationId")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddGuests(int reservationId, [FromBody] List<ReservationGuestDto> guestsDto)
        {
            if (guestsDto == null || !guestsDto.Any())
                return BadRequest(new { message = "Guest data is required." });

            var success = await _reservationGuestService.AddGuestsAsync(guestsDto, reservationId);

            if (!success)
                return BadRequest(new { message = "Could not add guests due to validation errors or database issues." });

            return Created();
        }
    }
}