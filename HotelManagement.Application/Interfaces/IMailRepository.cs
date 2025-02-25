using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for sending emails.
    /// </summary>
    public interface IMailRepository
    {
        /// <summary>
        /// Sends an email to the specified recipient.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
