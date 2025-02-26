using HotelManagement.Application.DTOs;
using HotelManagement.Application.Interfaces;
using HotelManagement.Domain.Entities;
namespace HotelManagement.Application.Services
{
    /// <summary>
    /// Service for managing room-related operations.
    /// </summary>
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomService"/> class.
        /// </summary>
        /// <param name="roomRepository">The repository responsible for room data access.</param>
        /// <param name="hotelRepository">The repository responsible for hotel data access.</param>
        public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository)
        {
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
        }

        /// <summary>
        /// Retrieves all rooms associated with a specific hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <returns>A list of basic room details for the specified hotel.</returns>
        public async Task<IEnumerable<RoomBasicDto>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _roomRepository.GetRoomsByHotelIdAsync(hotelId);
        }

        /// <summary>
        /// Assigns a set of rooms to a hotel.
        /// </summary>
        /// <param name="assignRoomsDto">The DTO containing the hotel ID and room details.</param>
        /// <returns>True if the rooms were successfully assigned; otherwise, false.</returns>
        public async Task<bool> AssignRoomsToHotelAsync(AssignRoomsDto assignRoomsDto)
        {
            var hotelExists = await _hotelRepository.GetByIdAsync(assignRoomsDto.HotelId);
            if (hotelExists == null)
            {
                return false;
            }

            var rooms = assignRoomsDto.Rooms.Select(r => new Room
            {
                Hotelid = assignRoomsDto.HotelId,
                Capacity = r.Capacity,
                Basecost = r.BaseCost,
                Taxes = r.Taxes,
                Isactive = r.IsActive ?? true,
                Location = r.Location,
                RoomType = r.RoomType
            }).ToList();

            await _roomRepository.AddRangeAsync(rooms);
            return true;
        }

        /// <summary>
        /// Updates the details of a specific room in a given hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel the room belongs to.</param>
        /// <param name="roomId">The ID of the room to update.</param>
        /// <param name="updateRoomDto">The DTO containing the updated room details.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public async Task<bool> UpdateRoomAsync(int hotelId, int roomId, RoomUpdateDto updateRoomDto)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null || room.Hotelid != hotelId)
                return false;

            room.Capacity = updateRoomDto.Capacity;
            room.Basecost = updateRoomDto.BaseCost;
            room.Taxes = updateRoomDto.Taxes;
            room.Isactive = updateRoomDto.IsActive;
            room.Location = updateRoomDto.Location;
            room.RoomType = updateRoomDto.RoomType;

            await _roomRepository.UpdateAsync(room);
            return true;
        }

        /// <summary>
        /// Searches for available rooms based on the provided criteria.
        /// </summary>
        /// <param name="searchDto">The DTO containing search filters such as dates, guests, and city.</param>
        /// <returns>A list of available rooms matching the search criteria.</returns>
        public async Task<IEnumerable<RoomBasicDto>> SearchRoomsAsync(SearchRoomsDto searchDto)
        {
            var checkInDate = searchDto.CheckIn.ToDateTime(TimeOnly.MinValue);  
            var checkOutDate = searchDto.CheckOut.ToDateTime(TimeOnly.MaxValue);

           
            if (checkOutDate <= checkInDate)
            {
                throw new ArgumentException("Check-out date must be after check-in date.");
            }

            
            if (checkInDate < DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Check-in date cannot be in the past.");
            }

            return await _roomRepository.SearchRoomsAsync(checkInDate, checkOutDate, searchDto.Guests, searchDto.City);
        }

        /// <summary>
        ///  Updates the status of a specific room in a given hotel.
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="roomId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRoomStatusAsync(int hotelId, int roomId, bool isActive)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null || room.Hotelid != hotelId)
                return false;

            room.Isactive = isActive;
            await _roomRepository.UpdateAsync(room);
            return true;
        }
    }
}
