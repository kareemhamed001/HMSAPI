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
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly ILogger<DoctorService> _logger;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository, ILogger<DoctorService> logger, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<DoctorResponse> CreateDoctorAsync(DoctorRequest doctorRequest)
        {
            try
            {
                Doctor doctor = _mapper.Map<Doctor>(doctorRequest);
                return await _doctorRepository.CreateDoctorAsync(doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating doctor with Description: {Description}", doctorRequest.Description);
                throw;
            }
        }

        public async Task<Doctor> DeleteDoctorAsync(int id)
        {
            try
            {
                var doctorResponse = await _doctorRepository.GetDoctorByIdAsync(id);
                if (doctorResponse == null)
                {
                    _logger.LogWarning("Doctor with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Doctor not found.");
                }
                Doctor doctor = _mapper.Map<Doctor>(doctorResponse);
                return await _doctorRepository.DeleteDoctorAsync(doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting doctor with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<DoctorResponse>> GetAllDoctorsAsync()
        {
            try
            {
                return await _doctorRepository.GetAllDoctorsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all doctors.");
                throw;
            }
        }

        public async Task<DoctorResponse> GetDoctorByIdAsync(int id)
        {
            try
            {
                var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
                if (doctor == null)
                {
                    _logger.LogWarning("Doctor with ID: {Id} not found.", id);
                    throw new NotFoundException("Doctor not found.");
                }
                return doctor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching doctor with ID: {Id}", id);
                throw;
            }
        }

        public async Task<DoctorResponse> UpdateDoctorAsync(int id, DoctorRequest doctorRequest)
        {
            try
            {
                var existingDoctor = await _doctorRepository.GetDoctorByIdAsync(id);
                if (existingDoctor == null)
                {
                    _logger.LogWarning("Doctor with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Doctor not found.");
                }

                Doctor doctor = _mapper.Map<Doctor>(existingDoctor);
                _mapper.Map(doctorRequest, doctor);
                return await _doctorRepository.UpdateDoctorAsync(id, doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating doctor with ID: {Id}", id);
                throw;
            }
        }
    }
}
