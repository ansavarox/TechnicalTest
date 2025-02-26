using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class ReservationDetailDto
    {
        [JsonPropertyName("reservationId")]
        public int Id { get; set; }
        public string HotelName { get; set; }
        public string RoomType { get; set; }
        public int RoomId { get; set; }
        public string TravelerName { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public decimal TotalCost { get; set; }
        public List<ReservationGuestDto> Guests { get; set; }
        public List<EmergencyContactDto> EmergencyContacts { get; set; }
    }
}
