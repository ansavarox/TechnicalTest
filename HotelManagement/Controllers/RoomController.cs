using HotelManagement.Application.DTOs;
using HotelManagement.Application.Services;
using HotelManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Api.Controllers
{
    /// <summary>
    /// Manages room-related operations, including retrieval, assignment, updates, and searches.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly HotelService _hotelService;
        private readonly RoomService _roomService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomController"/> class.
        /// </summary>
        /// <param name="hotelService">Service for hotel-related operations.</param>
        /// <param name="roomService">Service for room-related operations.</param>
        public RoomController(HotelService hotelService, RoomService roomService)
        {
            _hotelService = hotelService;
            _roomService = roomService;
        }

        /// <summary>
        /// Retrieves all rooms associated with a specific hotel.
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel.</param>
        /// <returns>A list of rooms for the specified hotel.</returns>
        /// <response code="200">Returns the list of rooms.</response>
        /// <response code="404">If no rooms are found for the given hotel.</response>
        [Authorize(Roles = "Agent")]
        [HttpGet("GetRoomsByHotelId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoomsByHotelId(int hotelId)
        {
            var rooms = await _roomService.GetRoomsByHotelIdAsync(hotelId);

            if (!rooms.Any())
                return NotFound(new { message = "No rooms found for this hotel" });

            return Ok(rooms);
        }

        /// <summary>
        /// Assigns rooms to a specific hotel.
        /// </summary>
        /// <param name="assignRoomsDto">Data transfer object containing room assignment details.</param>
        /// <returns>A response indicating the success of the operation.</returns>
        /// <response code="200">Rooms assigned successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="404">If the hotel is not found.</response>
        [Authorize(Roles = "Agent")]
        [HttpPost("AssignRoomsToHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignRoomsToHotel([FromBody] AssignRoomsDto assignRoomsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _roomService.AssignRoomsToHotelAsync(assignRoomsDto);

            if (!success)
            {
                return NotFound(new { message = "Hotel not found." });
            }

            return Ok(new { message = "Rooms assigned successfully." });
        }

        /// <summary>
        /// Updates details of a specific room within a hotel.
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel.</param>
        /// <param name="roomId">The unique identifier of the room.</param>
        /// <param name="updateRoomDto">The updated room details.</param>
        /// <returns>A response indicating the success of the operation.</returns>
        /// <response code="200">Room updated successfully.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="404">If the room is not found or does not belong to the specified hotel.</response>
        [Authorize(Roles = "Agent")]
        [HttpPatch("UpdateRoomByHotelId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoom(int hotelId, int roomId, [FromBody] RoomUpdateDto? updateRoomDto)
        {
            if (updateRoomDto == null)
                return BadRequest(new { message = "Invalid request body." });

            var updated = await _roomService.UpdateRoomAsync(hotelId, roomId, updateRoomDto);

            if (!updated)
                return NotFound(new { message = "Room not found or does not belong to the specified hotel." });

            return Ok(new { message = "Room updated successfully." });
        }

        /// <summary>
        /// Searches for available rooms based on specific criteria.
        /// </summary>
        /// <param name="searchDto">The search criteria for available rooms.</param>
        /// <returns>A list of rooms matching the criteria.</returns>
        /// <response code="200">Returns a list of available rooms.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="404">If no available rooms match the given criteria.</response>
        [Authorize(Roles = "Traveler")]
        [HttpPost("SearchRooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchRooms([FromBody] SearchRoomsDto searchDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rooms = await _roomService.SearchRoomsAsync(searchDto);

            if (!rooms.Any())
            {
                return NotFound(new { message = "No available rooms found for the given criteria." });
            }

            return Ok(rooms);
        }
    }
}
