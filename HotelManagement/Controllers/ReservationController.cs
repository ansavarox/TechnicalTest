using HotelManagement.Application.DTOs;
using HotelManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManagement.Api.Controllers
{
    /// <summary>
    /// Handles reservation-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;
        private readonly RoomService _roomService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationController"/> class.
        /// </summary>
        /// <param name="reservationService">The reservation service.</param>
        /// <param name="roomService">The room service.</param>
        public ReservationController(ReservationService reservationService, RoomService roomService)
        {
            _reservationService = reservationService;
            _roomService = roomService;
        }

        /// <summary>
        /// Retrieves all reservations for a specific hotel.
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel.</param>
        /// <returns>A list of reservations.</returns>
        /// <response code="200">Returns the list of reservations.</response>
        /// <response code="404">If no reservations are found.</response>
        [Authorize(Roles = "Agent")]
        [HttpGet("GetReservationsByHotelId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReservationsByHotel(int hotelId)
        {
            var reservations = await _reservationService.GetReservationsByHotelAsync(hotelId);

            if (!reservations.Any())
                return NotFound(new { message = "No reservations found for this hotel." });

            return Ok(reservations);
        }

        /// <summary>
        /// Retrieves reservation details by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the reservation.</param>
        /// <returns>The reservation details.</returns>
        /// <response code="200">Returns the reservation details.</response>
        /// <response code="404">If the reservation is not found.</response>
        [Authorize(Roles = "Agent")]
        [HttpGet("GetReservationsDetailsById")]
        [ProducesResponseType(typeof(ReservationDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReservationDetailDto>> GetReservationDetailsById(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
                return NotFound("Reservation not found.");

            return Ok(reservation);
        }

        /// <summary>
        /// Creates a new reservation.
        /// </summary>
        /// <param name="reservationDto">The reservation data.</param>
        /// <returns>A response indicating the reservation status.</returns>
        /// <response code="200">Returns the created reservation details.</response>
        /// <response code="400">If the request data is invalid or the room is not available.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [Authorize(Roles = "Traveler")]
        [HttpPost("CreateReservation")]
        [ProducesResponseType(typeof(ReservationDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto reservationDto)
        {
            if (reservationDto.CheckIn >= reservationDto.CheckOut)
            {
                return BadRequest("Check-in date must be before check-out date.");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User could not be identified.");
            }

            var isAvailable = await _reservationService.IsRoomAvailable(reservationDto.RoomId, reservationDto.CheckIn, reservationDto.CheckOut);
            if (!isAvailable)
            {
                return BadRequest("The selected room is not available for the specified dates.");
            }

            var createdReservation = await _reservationService.CreateReservation(
                int.Parse(userId), reservationDto.HotelId, reservationDto.RoomId, reservationDto.CheckIn, reservationDto.CheckOut);

            if (createdReservation == null)
            {
                return BadRequest("Reservation could not be created.");
            }

            var responseDto = await _reservationService.GetSimpleReservationByIdAsync(createdReservation.Id);

            return Ok(responseDto);
        }
    }
}
