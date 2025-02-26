using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class RoomUpdateDto
    {
        public int Capacity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Base cost must be greater than zero.")]
        public decimal BaseCost { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "Taxes cannot be negative.")]
        public decimal Taxes { get; set; }

        public bool IsActive { get; set; }

        public string Location { get; set; }

        public string RoomType { get; set; }
    }
}
