using HotelManagement.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelManagement.Application.DTOs;
using HotelManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagement.Api.Controllers
{
    /// <summary>
    /// Handles email-related operations for reservations.
    /// </summary>
    [Authorize(Roles = "Agent")]
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly ReservationService _reservationService;
        private readonly MailService _mailService;
        private readonly IMailRepository _mailRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="MailController"/> class.
        /// </summary>
        /// <param name="reservationService">The reservation service.</param>
        /// <param name="mailRepository">The mail repository responsible for sending emails.</param>
        /// <param name="mailService">The mail service for generating email content.</param>
        public MailController(ReservationService reservationService, IMailRepository mailRepository, MailService mailService    )
        {
            _reservationService = reservationService;
            _mailRepository = mailRepository;
            _mailService = mailService;
        }


        /// <summary>
        /// Sends reservation details via email.
        /// </summary>
        /// <param name="reservationId">The unique identifier of the reservation.</param>
        /// <returns>A response indicating the email status.</returns>
        /// <response code="200">Indicates that the email was sent successfully.</response>
        /// <response code="404">If the reservation is not found.</response>
        [HttpPost("SendReservationDetailsByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendReservationDetailsByEmail(int reservationId)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(reservationId);
            if (reservation == null)
                return NotFound(new { message = "Reservation not found." });

            string emailBody = _mailService.GenerateEmailBody(reservation);
            string subject = "Hotel Reservation Details";

            await _mailRepository.SendEmailAsync("recipient@example.com", subject, emailBody);

            return Ok(new { message = "Reservation details sent via email successfully." });
        }

    }
}
