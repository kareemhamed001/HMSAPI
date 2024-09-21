using AutoMapper;
using BusinessLayer.Requests;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Logging;
using SharedClasses.Exceptions;
using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<PatientService> _logger;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository patientRepository, ILogger<PatientService> logger, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PatientResponse> CreatePatientAsync(PatientRequest patientRequest)
        {
            try
            {
                var patient = _mapper.Map<Patient>(patientRequest);
                return await _patientRepository.CreatePatientAsync(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating patient.");
                throw;
            }
        }

        public async Task<Patient> DeletePatientAsync(int id)
        {
            try
            {
                var patientResponse = await _patientRepository.GetPatientById(id);
                if (patientResponse == null)
                {
                    _logger.LogWarning("Patient with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Patient not found.");
                }
                var patient = _mapper.Map<Patient>(patientResponse);
                return await _patientRepository.DeletePatientAsync(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting patient with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<PatientResponse>> GetAllPatientsAsync()
        {
            try
            {
                return await _patientRepository.GetAllPatientsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all patients.");
                throw;
            }
        }

        public async Task<PatientResponse> GetPatientByIdAsync(int id)
        {
            try
            {
                var patient = await _patientRepository.GetPatientById(id);
                if (patient == null)
                {
                    _logger.LogWarning("Patient with ID: {Id} not found.", id);
                    throw new NotFoundException("Patient not found.");
                }
                return patient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching patient with ID: {Id}", id);
                throw;
            }
        }

        public async Task<PatientResponse> UpdatePatientAsync(int id, PatientRequest patientRequest)
        {
            try
            {
                var existingPatient = await _patientRepository.GetPatientById(id);
                if (existingPatient == null)
                {
                    _logger.LogWarning("Patient with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Patient not found.");
                }

                var patient = _mapper.Map<Patient>(existingPatient);
                _mapper.Map(patientRequest, patient);
                return await _patientRepository.UpdatePatientAsync(id, patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating patient with ID: {Id}", id);
                throw;
            }
        }
    }
}
