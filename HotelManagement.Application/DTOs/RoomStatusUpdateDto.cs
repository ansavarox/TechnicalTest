using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class RoomStatusUpdateDto
    {
        [Required]
        public bool IsActive { get; set; }
    }
}
