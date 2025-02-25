using HotelManagement.Application.DTOs;
using HotelManagement.Application.Interfaces;
using HotelManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Application.Services
{
    /// <summary>
    /// Service for handling hotel reservations.
    /// </summary>
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationService"/> class.
        /// </summary>
        /// <param name="reservationRepository">The repository responsible for reservation data access.</param>
        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        /// <summary>
        /// Retrieves all reservations for a given hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <returns>A list of reservations for the specified hotel.</returns>
        public async Task<IEnumerable<ReservationDto>> GetReservationsByHotelAsync(int hotelId)
        {
            var reservations = await _reservationRepository.GetByHotelIdAsync(hotelId);

            return reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                HotelId = r.Hotelid,
                RoomId = r.Roomid,
                TravelerId = r.Travelerid,
                CheckInDate = r.Checkindate.ToDateTime(TimeOnly.MinValue),
                CheckOutDate = r.Checkoutdate.ToDateTime(TimeOnly.MinValue),
                TotalCost = r.Totalcost
            });
        }

        /// <summary>
        /// Retrieves detailed information of a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation.</param>
        /// <returns>The reservation details.</returns>
        public async Task<ReservationDetailDto> GetReservationByIdAsync(int id)
        {
            return await _reservationRepository.GetReservationByIdAsync(id);
        }

        /// <summary>
        /// Retrieves a simplified version of a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation.</param>
        /// <returns>A simplified reservation detail response, or null if not found.</returns>
        public async Task<ReservationDetailResponseDto?> GetSimpleReservationByIdAsync(int id)
        {
            return await _reservationRepository.GetSimpleReservationByIdAsync(id);
        }

        /// <summary>
        /// Checks if a specific room is available for the given date range.
        /// </summary>
        /// <param name="roomId">The ID of the room.</param>
        /// <param name="checkIn">The check-in date.</param>
        /// <param name="checkOut">The check-out date.</param>
        /// <returns>True if the room is available; otherwise, false.</returns>
        public async Task<bool> IsRoomAvailable(int roomId, DateOnly checkIn, DateOnly checkOut)
        {
            return await _reservationRepository.IsRoomAvailable(roomId, checkIn, checkOut);
        }

        /// <summary>
        /// Creates a new reservation for a user in a specified hotel and room.
        /// </summary>
        /// <param name="userId">The ID of the traveler making the reservation.</param>
        /// <param name="hotelId">The ID of the hotel where the reservation is made.</param>
        /// <param name="roomId">The ID of the room being reserved.</param>
        /// <param name="checkIn">The check-in date.</param>
        /// <param name="checkOut">The check-out date.</param>
        /// <returns>The created reservation object.</returns>
        public async Task<Reservation> CreateReservation(int userId, int hotelId, int roomId, DateOnly checkIn, DateOnly checkOut)
        {
            var reservation = new Reservation
            {
                Travelerid = userId,
                Hotelid = hotelId,
                Roomid = roomId,
                Checkindate = checkIn,
                Checkoutdate = checkOut
            };

            return await _reservationRepository.AddReservation(reservation);
        }
    }
}
