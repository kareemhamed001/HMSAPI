using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Requests;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Logging;
using SharedClasses.Exceptions;
using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ILogger<ReservationService> _logger;
        private readonly IMapper _mapper;

        public ReservationService(IReservationRepository reservationRepository, ILogger<ReservationService> logger, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ReservationResponse> CreateReservationAsync(ReservationRequest reservationRequest)
        {
            try
            {
                Reservation reservation = _mapper.Map<Reservation>(reservationRequest);
                return await _reservationRepository.CreateReservationAsync(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating reservation for PatientId: {PatientId}", reservationRequest.PatientId);
                throw;
            }
        }

        public async Task<Reservation> DeleteReservationAsync(int id)
        {
            try
            {
                var reservationResponse = await _reservationRepository.GetReservationByIdAsync(id);
                if (reservationResponse == null)
                {
                    _logger.LogWarning("Reservation with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Reservation not found.");
                }
                Reservation reservation = _mapper.Map<Reservation>(reservationResponse);
                return await _reservationRepository.DeleteReservationAsync(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting reservation with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ReservationResponse>> GetAllReservationsAsync()
        {
            try
            {
                return await _reservationRepository.GetAllReservationsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all reservations.");
                throw;
            }
        }

        public async Task<ReservationResponse> GetReservationByIdAsync(int id)
        {
            try
            {
                var reservation = await _reservationRepository.GetReservationByIdAsync(id);
                if (reservation == null)
                {
                    _logger.LogWarning("Reservation with ID: {Id} not found.", id);
                    throw new NotFoundException("Reservation not found.");
                }
                return reservation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reservation with ID: {Id}", id);
                throw;
            }
        }

        public async Task<ReservationResponse> UpdateReservationAsync(int id, ReservationRequest reservationRequest)
        {
            try
            {
                var existingReservation = await _reservationRepository.GetReservationByIdAsync(id);
                if (existingReservation == null)
                {
                    _logger.LogWarning("Reservation with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Reservation not found.");
                }

                Reservation reservation = _mapper.Map<Reservation>(existingReservation);
                _mapper.Map(reservationRequest, reservation);
                return await _reservationRepository.UpdateReservationAsync(id, reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating reservation with ID: {Id}", id);
                throw;
            }
        }
    }
}
