using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class HotelUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(255, ErrorMessage = "Name must not exceed 255 characters.")]
        public string? Name { get; set; } 

        public string? Location { get; set; }

        public bool? IsActive { get; set; }
    }
}
