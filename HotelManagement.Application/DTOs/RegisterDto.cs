using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "The name field is required.")]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "The email field is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The password field is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "The role field is required.")]
        public string Role { get; set; }
    }
}
