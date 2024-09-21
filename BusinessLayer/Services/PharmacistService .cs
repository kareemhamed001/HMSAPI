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
    public class PharmacistService : IPharmacistService
    {
        private readonly IPharmacistRepository _pharmacistRepository;
        private readonly ILogger<PharmacistService> _logger;
        private readonly IMapper _mapper;

        public PharmacistService(IPharmacistRepository pharmacistRepository, ILogger<PharmacistService> logger, IMapper mapper)
        {
            _pharmacistRepository = pharmacistRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PharmacistResponse> CreatePharmacistAsync(PharmacistRequest pharmacistRequest)
        {
            try
            {
                Pharmacist pharmacist = _mapper.Map<Pharmacist>(pharmacistRequest);
                return await _pharmacistRepository.CreatePharmacistAsync(pharmacist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating pharmacist with UserId: {UserId}", pharmacistRequest.UserId);
                throw;
            }
        }

        public async Task<Pharmacist> DeletePharmacistAsync(int id)
        {
            try
            {
                var pharmacistResponse = await _pharmacistRepository.GetPharmacistByIdAsync(id);
                if (pharmacistResponse == null)
                {
                    _logger.LogWarning("Pharmacist with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Pharmacist not found.");
                }
                Pharmacist pharmacist = _mapper.Map<Pharmacist>(pharmacistResponse);
                return await _pharmacistRepository.DeletePharmacistAsync(pharmacist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting pharmacist with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<PharmacistResponse>> GetAllPharmacistsAsync()
        {
            try
            {
                return await _pharmacistRepository.GetAllPharmacistsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all pharmacists.");
                throw;
            }
        }

        public async Task<PharmacistResponse> GetPharmacistByIdAsync(int id)
        {
            try
            {
                var pharmacist = await _pharmacistRepository.GetPharmacistByIdAsync(id);
                if (pharmacist == null)
                {
                    _logger.LogWarning("Pharmacist with ID: {Id} not found.", id);
                    throw new NotFoundException("Pharmacist not found.");
                }
                return pharmacist;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching pharmacist with ID: {Id}", id);
                throw;
            }
        }

        public async Task<PharmacistResponse> UpdatePharmacistAsync(int id, PharmacistRequest pharmacistRequest)
        {
            try
            {
                var existingPharmacist = await _pharmacistRepository.GetPharmacistByIdAsync(id);
                if (existingPharmacist == null)
                {
                    _logger.LogWarning("Pharmacist with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Pharmacist not found.");
                }

                Pharmacist pharmacist = _mapper.Map<Pharmacist>(existingPharmacist);
                _mapper.Map(pharmacistRequest, pharmacist);
                return await _pharmacistRepository.UpdatePharmacistAsync(id, pharmacist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating pharmacist with ID: {Id}", id);
                throw;
            }
        }
    }
}
