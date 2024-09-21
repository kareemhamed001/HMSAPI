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
    public class SpecializationService : ISpecializationService
    {
        private readonly ISpecializationRepository _specializationRepository;
        private readonly ILogger<SpecializationService> _logger;
        private readonly IMapper _mapper;

        public SpecializationService(ISpecializationRepository specializationRepository, ILogger<SpecializationService> logger, IMapper mapper)
        {
            _specializationRepository = specializationRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SpecializationResponse> CreateSpecializationAsync(SpecializationRequest specializationRequest)
        {
            try
            {
                Specialization specialization = _mapper.Map<Specialization>(specializationRequest);
                return await _specializationRepository.CreateSpecializationAsync(specialization);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating specialization with Name: {Name}", specializationRequest.Name);
                throw;
            }
        }

        public async Task<Specialization> DeleteSpecializationAsync(int id)
        {
            try
            {
                var specializationResponse = await _specializationRepository.GetSpecializationByIdAsync(id);
                if (specializationResponse == null)
                {
                    _logger.LogWarning("Specialization with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Specialization not found.");
                }

                return await _specializationRepository.DeleteSpecializationAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Specialization with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<SpecializationResponse>> GetAllSpecializationsAsync()
        {
            try
            {
                return await _specializationRepository.GetAllSpecializationsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all specializations.");
                throw;
            }
        }

        public async Task<SpecializationResponse> GetSpecializationByIdAsync(int id)
        {
            try
            {
                var specialization = await _specializationRepository.GetSpecializationByIdAsync(id);
                if (specialization == null)
                {
                    _logger.LogWarning("Specialization with ID: {Id} not found.", id);
                    throw new NotFoundException("Specialization not found.");
                }
                return specialization;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching specialization with ID: {Id}", id);
                throw;
            }
        }

        public async Task<SpecializationResponse> UpdateSpecializationAsync(int id, SpecializationRequest specializationRequest)
        {
            try
            {
                var existingSpecialization = await _specializationRepository.GetSpecializationByIdAsync(id);
                if (existingSpecialization == null)
                {
                    _logger.LogWarning("Specialization with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Specialization not found.");
                }

                Specialization specialization = _mapper.Map<Specialization>(existingSpecialization);
                _mapper.Map(specializationRequest, specialization);

                return await _specializationRepository.UpdateSpecializationAsync(id, specialization);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Specialization with ID: {Id}", id);
                throw;
            }
        }
    }
}
