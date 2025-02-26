using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public class RoomBasicDto
    {
        [JsonPropertyName("roomId")]
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int Capacity { get; set; }
        public decimal BaseCost { get; set; }
        public decimal Taxes { get; set; }
        public bool IsActive { get; set; }
        public string Location { get; set; }
        public string RoomType { get; set; }
    }
}
