using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class HotelCreateDto
    {
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Location field is required.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "The IsActive field is required.")]
        public bool? IsActive { get; set; } = true;
    }
}
