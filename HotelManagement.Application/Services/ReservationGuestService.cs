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
    /// Service for managing guests associated with a reservation.
    /// </summary>
    public class ReservationGuestService
    {
        private readonly IReservationGuestRepository _reservationGuestRepository;
        private readonly IRoomRepository _roomRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationGuestService"/> class.
        /// </summary>
        /// <param name="reservationGuestRepository">The repository for handling reservation guests.</param>
        /// <param name="roomRepository">The repository for handling room-related operations.</param>
        public ReservationGuestService(IReservationGuestRepository reservationGuestRepository, IRoomRepository roomRepository)
        {
            _reservationGuestRepository = reservationGuestRepository;
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// Adds guests to a reservation, ensuring that the total number does not exceed room capacity.
        /// </summary>
        /// <param name="guestsDto">A list of guests to be added.</param>
        /// <param name="reservationId">The ID of the reservation.</param>
        /// <returns>A boolean indicating whether the guests were successfully added.</returns>
        /// <exception cref="InvalidOperationException">Thrown if adding the guests exceeds the room's capacity.</exception>
        public async Task<bool> AddGuestsAsync(List<ReservationGuestDto> guestsDto, int reservationId)
        {
            int currentGuestCount = await _reservationGuestRepository.GetGuestCountByReservationIdAsync(reservationId);

            int roomCapacity = await _roomRepository.GetRoomCapacityByReservationIdAsync(reservationId);
           
            if (currentGuestCount + guestsDto.Count > roomCapacity)
            {
                throw new InvalidOperationException($"This room can only accommodate {roomCapacity} additional guests.");
            }

            var guests = guestsDto.Select(g => new Reservationguest
            {
                Fullname = g.FullName,
                Birthdate = g.BirthDate,
                Gender = g.Gender,
                Documenttype = g.DocumentType,
                Documentnumber = g.DocumentNumber,
                Email = g.Email,
                Phone = g.Phone,
                Reservationid = reservationId
            }).ToList();

            return await _reservationGuestRepository.AddGuestsAsync(guests);
        }

        /// <summary>
        /// Retrieves the number of guests currently assigned to a specific reservation.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation.</param>
        /// <returns>The total number of guests in the reservation.</returns>
        public async Task<int> GetGuestCountByReservationIdAsync(int reservationId)
        {
            return await _reservationGuestRepository.GetGuestCountByReservationIdAsync(reservationId);
        }

    }
}
