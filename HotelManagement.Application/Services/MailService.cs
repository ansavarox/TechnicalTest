using HotelManagement.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using HotelManagement.Application.DTOs;

namespace HotelManagement.Application.Services
{
    /// <summary>
    /// Service for sending emails and generating email content.
    /// </summary>
    public class MailService : IMailRepository
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration containing email settings.</param>
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Sends an email asynchronously using SMTP settings from the application configuration.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The HTML body content of the email.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", "your-email@example.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            var client = new SmtpClient();
            await client.ConnectAsync(_configuration["MailSettings:Host"], Convert.ToInt32(_configuration["MailSettings:Port"]), false);
            await client.AuthenticateAsync(_configuration["MailSettings:Username"], _configuration["MailSettings:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            client.Dispose();
        }

        /// <summary>
        /// Generates an HTML email body with reservation details.
        /// </summary>
        /// <param name="reservationDetailDto">The reservation details to include in the email.</param>
        /// <returns>A formatted HTML string containing the reservation details.</returns>
        public string GenerateEmailBody(ReservationDetailDto reservationDetailDto)
        {
            var emailBody = @"
        <html>
            <head>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        color: #333;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }
                    .container {
                        width: 600px;
                        margin: 20px auto;
                        background-color: #ffffff;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    }
                    h1 {
                        color: #007bff;
                        font-size: 24px;
                        text-align: center;
                    }
                    h2 {
                        color: #333;
                        font-size: 20px;
                        margin-top: 20px;
                        border-bottom: 2px solid #007bff;
                        padding-bottom: 5px;
                    }
                    p {
                        font-size: 14px;
                        line-height: 1.6;
                    }
                    .guest-info, .emergency-info {
                        background-color: #f9f9f9;
                        padding: 10px;
                        margin-top: 10px;
                        border-radius: 5px;
                        border: 1px solid #ddd;
                    }
                    .guest-info p, .emergency-info p {
                        margin: 5px 0;
                    }
                    .footer {
                        text-align: center;
                        font-size: 12px;
                        color: #aaa;
                        margin-top: 20px;
                    }
                    .footer a {
                        color: #007bff;
                        text-decoration: none;
                    }
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>Hotel Reservation Details</h1>
                    <p><strong>Hotel:</strong> " + reservationDetailDto.HotelName + @"</p>
                    <p><strong>Room Type:</strong> " + reservationDetailDto.RoomType + @"</p>
                    <p><strong>Traveler Name:</strong> " + reservationDetailDto.TravelerName + @"</p>
                    <p><strong>Check-in:</strong> " + reservationDetailDto.CheckInDate.ToString("yyyy-MM-dd") + @"</p>
                    <p><strong>Check-out:</strong> " + reservationDetailDto.CheckOutDate.ToString("yyyy-MM-dd") + @"</p>

                    <h2>Guests</h2>";

            // Add each guest's information in a styled section
            foreach (var guest in reservationDetailDto.Guests)
            {
                emailBody += $@"
            <div class='guest-info'>
                <p><strong>Name:</strong> {guest.FullName}</p>
                <p><strong>Date of Birth:</strong> {guest.BirthDate.ToString("yyyy-MM-dd")}</p>
                <p><strong>Gender:</strong> {guest.Gender}</p>
                <p><strong>Document Type:</strong> {guest.DocumentType}</p>
                <p><strong>Document Number:</strong> {guest.DocumentNumber}</p>
                <p><strong>Email:</strong> {guest.Email}</p>
                <p><strong>Phone:</strong> {guest.Phone}</p>
            </div>";
            }

            emailBody += "<h2>Emergency Contacts</h2>";

            // Add each emergency contact's information
            foreach (var emergencyContact in reservationDetailDto.EmergencyContacts)
            {
                emailBody += $@"
            <div class='emergency-info'>
                <p><strong>Name:</strong> {emergencyContact.FullName}</p>
                <p><strong>Phone:</strong> {emergencyContact.Phone}</p>
            </div>";
            }

            // Closing the HTML structure with a footer
            emailBody += @"
                <div class='footer'>
                    <p>This is an automated email. Please do not reply.</p>
                    <p><a href='https://www.google.com'>Visit our website</a></p>
                </div>
            </body>
        </html>";

            return emailBody;
        }
    }
}
