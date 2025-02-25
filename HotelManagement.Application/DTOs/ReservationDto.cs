using HotelManagement.Application.DTOs.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class ReservationDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int HotelId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int TravelerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DateGreaterThan(nameof(CheckInDate), ErrorMessage = "Check-out date must be later than check-in date.")]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total cost must be greater than zero.")]
        public decimal TotalCost { get; set; }
    }
}
