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
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepository _clinicRepository;
        private readonly ILogger<ClinicService> _logger;
        private readonly IMapper _mapper;

        public ClinicService(IClinicRepository clinicRepository, ILogger<ClinicService> logger, IMapper mapper)
        {
            _clinicRepository = clinicRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ClinicResponse> CreateClinicAsync(ClinicRequest clinicRequest)
        {
            try
            {
                Clinic clinic = _mapper.Map<Clinic>(clinicRequest);
                return await _clinicRepository.CreateClinicAsync(clinic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating clinic with Name: {Name}", clinicRequest.Name);
                throw;
            }
        }

        public async Task<Clinic> DeleteClinicAsync(int id)
        {
            try
            {
                var clinicResponse = await _clinicRepository.GetClinicByIdAsync(id);
                if (clinicResponse == null)
                {
                    _logger.LogWarning("Clinic with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Clinic not found.");
                }
                return await _clinicRepository.DeleteClinicAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Clinic with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ClinicResponse>> GetAllClinicsAsync()
        {
            try
            {
                return await _clinicRepository.GetAllClinicsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all clinics.");
                throw;
            }
        }

        public async Task<ClinicResponse> GetClinicByIdAsync(int id)
        {
            try
            {
                var clinic = await _clinicRepository.GetClinicByIdAsync(id);
                if (clinic == null)
                {
                    _logger.LogWarning("Clinic with ID: {Id} not found.", id);
                    throw new NotFoundException("Clinic not found.");
                }
                return clinic;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching clinic with ID: {Id}", id);
                throw;
            }
        }

        public async Task<ClinicResponse> UpdateClinicAsync(int id, ClinicRequest clinicRequest)
        {
            try
            {
                var existingClinic = await _clinicRepository.GetClinicByIdAsync(id);
                if (existingClinic == null)
                {
                    _logger.LogWarning("Clinic with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Clinic not found.");
                }

                Clinic clinic = _mapper.Map<Clinic>(existingClinic);
                _mapper.Map(clinicRequest, clinic);

                return await _clinicRepository.UpdateClinicAsync(id, clinic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Clinic with ID: {Id}", id);
                throw;
            }
        }
    }
}
