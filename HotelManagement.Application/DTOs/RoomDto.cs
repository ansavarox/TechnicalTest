using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class RoomDto
    {
        [Required(ErrorMessage = "Room capacity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Base cost is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Base cost must be a positive value.")]
        public decimal BaseCost { get; set; }

        [Required(ErrorMessage = "Taxes are required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Taxes must be a positive value.")]
        public decimal Taxes { get; set; }

        [Required(ErrorMessage = "Room availability is required.")]
        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "Room location is required.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Room type is required.")]
        public string RoomType { get; set; } 
    }
}
