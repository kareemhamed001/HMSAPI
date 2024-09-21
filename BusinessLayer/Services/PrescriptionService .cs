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
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly ILogger<PrescriptionService> _logger;
        private readonly IMapper _mapper;

        public PrescriptionService(IPrescriptionRepository prescriptionRepository, ILogger<PrescriptionService> logger, IMapper mapper)
        {
            _prescriptionRepository = prescriptionRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PrescriptionResponse> CreatePrescriptionAsync(PrescriptionRequest prescriptionRequest)
        {
            try
            {
                Prescription prescription = _mapper.Map<Prescription>(prescriptionRequest);
                return await _prescriptionRepository.CreatePrescriptionAsync(prescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating prescription for PatientId: {PatientId}", prescriptionRequest.PatientId);
                throw;
            }
        }

        public async Task<Prescription> DeletePrescriptionAsync(int id)
        {
            try
            {
                var prescriptionResponse = await _prescriptionRepository.GetPrescriptionByIdAsync(id);
                if (prescriptionResponse == null)
                {
                    _logger.LogWarning("Prescription with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Prescription not found.");
                }
                Prescription prescription = _mapper.Map<Prescription>(prescriptionResponse);
                return await _prescriptionRepository.DeletePrescriptionAsync(prescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting prescription with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync()
        {
            try
            {
                return await _prescriptionRepository.GetAllPrescriptionsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all prescriptions.");
                throw;
            }
        }

        public async Task<PrescriptionResponse> GetPrescriptionByIdAsync(int id)
        {
            try
            {
                var prescription = await _prescriptionRepository.GetPrescriptionByIdAsync(id);
                if (prescription == null)
                {
                    _logger.LogWarning("Prescription with ID: {Id} not found.", id);
                    throw new NotFoundException("Prescription not found.");
                }
                return prescription;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching prescription with ID: {Id}", id);
                throw;
            }
        }

        public async Task<PrescriptionResponse> UpdatePrescriptionAsync(int id, PrescriptionRequest prescriptionRequest)
        {
            try
            {
                var existingPrescription = await _prescriptionRepository.GetPrescriptionByIdAsync(id);
                if (existingPrescription == null)
                {
                    _logger.LogWarning("Prescription with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Prescription not found.");
                }

                Prescription prescription = _mapper.Map<Prescription>(existingPrescription);
                _mapper.Map(prescriptionRequest, prescription);
                return await _prescriptionRepository.UpdatePrescriptionAsync(id, prescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating prescription with ID: {Id}", id);
                throw;
            }
        }
    }
}
