using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class CreateReservationDto
    {
        [Required(ErrorMessage = "The Hotel ID is required.")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "The Room ID is required.")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "CheckIn Date is required.")]
        public DateOnly CheckIn { get; set; }

        [Required(ErrorMessage = "CheckOut Date is required.")]
        public DateOnly CheckOut { get; set; }
    }
}
