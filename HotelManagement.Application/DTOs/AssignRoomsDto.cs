using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class AssignRoomsDto
    {
        [Required(ErrorMessage = "Hotel ID is required.")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "At least one room must be provided.")]
        public List<RoomDto> Rooms { get; set; } = new List<RoomDto>();
    }
}
