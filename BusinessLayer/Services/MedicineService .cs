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
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly ILogger<MedicineService> _logger;
        private readonly IMapper _mapper;

        public MedicineService(IMedicineRepository medicineRepository, ILogger<MedicineService> logger, IMapper mapper)
        {
            _medicineRepository = medicineRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<MedicineResponse> CreateMedicineAsync(MedicineRequest medicineRequest)
        {
            try
            {
                Medicine medicine = _mapper.Map<Medicine>(medicineRequest);
                return await _medicineRepository.CreateMedicineAsync(medicine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating medicine with Name: {Name}", medicineRequest.Name);
                throw;
            }
        }

        public async Task<Medicine> DeleteMedicineAsync(int id)
        {
            try
            {
                var medicineResponse = await _medicineRepository.GetMedicineByIdAsync(id);
                if (medicineResponse == null)
                {
                    _logger.LogWarning("Medicine with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Medicine not found.");
                }

                return await _medicineRepository.DeleteMedicineAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Medicine with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<MedicineResponse>> GetAllMedicinesAsync()
        {
            try
            {
                return await _medicineRepository.GetAllMedicinesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all medicines.");
                throw;
            }
        }

        public async Task<MedicineResponse> GetMedicineByIdAsync(int id)
        {
            try
            {
                var medicine = await _medicineRepository.GetMedicineByIdAsync(id);
                if (medicine == null)
                {
                    _logger.LogWarning("Medicine with ID: {Id} not found.", id);
                    throw new NotFoundException("Medicine not found.");
                }
                return medicine;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching medicine with ID: {Id}", id);
                throw;
            }
        }

        public async Task<MedicineResponse> UpdateMedicineAsync(int id, MedicineRequest medicineRequest)
        {
            try
            {
                var existingMedicine = await _medicineRepository.GetMedicineByIdAsync(id);
                if (existingMedicine == null)
                {
                    _logger.LogWarning("Medicine with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Medicine not found.");
                }

                Medicine medicine = _mapper.Map<Medicine>(existingMedicine);
                _mapper.Map(medicineRequest, medicine);

                return await _medicineRepository.UpdateMedicineAsync(id, medicine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Medicine with ID: {Id}", id);
                throw;
            }
        }
    }
}
