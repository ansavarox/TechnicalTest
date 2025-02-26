using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class ReservationDetailResponseDto
    {
        [JsonPropertyName("reservationId")]
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public int TravelerId { get; set; }
        public string TravelerName { get; set; } = string.Empty;
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
