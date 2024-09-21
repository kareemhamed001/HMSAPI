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
    public class NurseService : INurseService
    {
        private readonly INurseRepository _nurseRepository;
        private readonly ILogger<NurseService> _logger;
        private readonly IMapper _mapper;

        public NurseService(INurseRepository nurseRepository, ILogger<NurseService> logger, IMapper mapper)
        {
            _nurseRepository = nurseRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<NurseResponse> CreateNurseAsync(NurseRequest nurseRequest)
        {
            try
            {
                Nurse nurse = _mapper.Map<Nurse>(nurseRequest);
                return await _nurseRepository.CreateNurseAsync(nurse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating nurse with Passport: {Passport}", nurseRequest.Passport);
                throw;
            }
        }

        public async Task<Nurse> DeleteNurseAsync(int id)
        {
            try
            {
                var nurseResponse = await _nurseRepository.GetNurseByIdAsync(id);
                if (nurseResponse == null)
                {
                    _logger.LogWarning("Nurse with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Nurse not found.");
                }
                return await _nurseRepository.DeleteNurseAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Nurse with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<NurseResponse>> GetAllNursesAsync()
        {
            try
            {
                return await _nurseRepository.GetAllNursesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all nurses.");
                throw;
            }
        }

        public async Task<NurseResponse> GetNurseByIdAsync(int id)
        {
            try
            {
                var nurse = await _nurseRepository.GetNurseByIdAsync(id);
                if (nurse == null)
                {
                    _logger.LogWarning("Nurse with ID: {Id} not found.", id);
                    throw new NotFoundException("Nurse not found.");
                }
                return nurse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching nurse with ID: {Id}", id);
                throw;
            }
        }

        public async Task<NurseResponse> UpdateNurseAsync(int id, NurseRequest nurseRequest)
        {
            try
            {
                var existingNurse = await _nurseRepository.GetNurseByIdAsync(id);
                if (existingNurse == null)
                {
                    _logger.LogWarning("Nurse with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Nurse not found.");
                }

                Nurse nurse = _mapper.Map<Nurse>(existingNurse);
                _mapper.Map(nurseRequest, nurse);

                return await _nurseRepository.UpdateNurseAsync(id, nurse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Nurse with ID: {Id}", id);
                throw;
            }
        }
    }
}
