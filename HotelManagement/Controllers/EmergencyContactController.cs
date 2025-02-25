using HotelManagement.Application.DTOs;
using HotelManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Api.Controllers
{   
    /// <summary>
    /// Manages emergency contact information for reservations.
    /// </summary>
    [Authorize(Roles = "Traveler")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyContactController : ControllerBase
    {
        private readonly EmergencyContactService _contactService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmergencyContactController"/> class.
        /// </summary>
        /// <param name="contactService">The emergency contact service.</param>
        public EmergencyContactController(EmergencyContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Retrieves the emergency contact associated with a reservation.
        /// </summary>
        /// <param name="reservationId">The unique identifier of the reservation.</param>
        /// <returns>The emergency contact details if found.</returns>
        /// <response code="200">Returns the emergency contact information.</response>
        /// <response code="404">If no emergency contact is found for the given reservation.</response>
        [HttpGet("GetEmergencyContactByReservationId")]
        [ProducesResponseType(typeof(EmergencyContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmergencyContactDto>> GetEmergencyContact(int reservationId)
        {
            var contact = await _contactService.GetByReservationIdAsync(reservationId);
            if (contact == null) return NotFound(new { message = "Emergency contact not found." });
            return Ok(contact);
        }

        /// <summary>
        /// Adds or updates the emergency contact information for a reservation.
        /// </summary>
        /// <param name="reservationId">The unique identifier of the reservation.</param>
        /// <param name="contactDto">The emergency contact details.</param>
        /// <returns>No content if the operation is successful.</returns>
        /// <response code="204">Indicates the contact was successfully added or updated.</response>
        /// <response code="400">If the contact data is missing.</response>
        /// <response code="404">If the reservation is not found.</response>
        [HttpPost("AddOrUpdateEmergencyContact")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddOrUpdateEmergencyContact(int reservationId, [FromBody] EmergencyContactDto contactDto)
        {
            if (contactDto == null)
                return BadRequest(new { message = "Contact data is required." });

            var success = await _contactService.AddOrUpdateAsync(reservationId, contactDto);

            if (!success)
                return NotFound(new { message = "Reservation not found." });

            return NoContent();
        }
    }
}
