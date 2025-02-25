using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "The email is required.")]
        public string Email { get; set; } = null!; 
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;
    }
}
