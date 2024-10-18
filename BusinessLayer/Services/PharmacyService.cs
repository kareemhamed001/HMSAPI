using AutoMapper;
using BusinessLayer.Requests;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Logging;
using SharedClasses.Exceptions;
using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class PharmacyService : IPharmacyService
    {
        private readonly IPharmacyRepository _pharmacyRepository;
        private readonly IMedicineRepository _medicineRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<PharmacyService> _logger;
        private readonly IMapper mapper;

        public PharmacyService(IPharmacyRepository pharmacyRepository, ILogger<PharmacyService> logger, IMapper mapper,
            IMedicineRepository medicineRepository, IRoomRepository roomRepository)
        {
            _pharmacyRepository = pharmacyRepository;
            _logger = logger;
            this.mapper = mapper;
            _medicineRepository = medicineRepository;
            _roomRepository = roomRepository;
        }

        public async Task<PharmacyResponse> CreatePharmacyAsync(PharmacyRequest pharmacyRequest)
        {
            try
            {
                var room = await _roomRepository.GetRoomById(pharmacyRequest.RoomId);
                if (room == null)
                {
                    _logger.LogWarning("Room with ID: {Id} not found for Pharmacy creation.", pharmacyRequest.RoomId);
                    throw new NotFoundException("Room not found.");
                }

                var roomIsAvailable = await _roomRepository.RoomIsAvailableAsync(pharmacyRequest.RoomId);
                if (!roomIsAvailable)
                {
                    _logger.LogWarning("Room with ID: {Id} is already occupied.", pharmacyRequest.RoomId);
                    throw new Exception("Room is already occupied.");
                }

                Pharmacy pharmacy = mapper.Map<Pharmacy>(pharmacyRequest);

                return await _pharmacyRepository.CreatePharmacyAsync(pharmacy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating room with Name: {Name}", pharmacyRequest.Name);
                throw;
            }
        }

        public async Task<Pharmacy> DeletePharmacyAsync(int id)
        {
            try
            {
                var pharmacyResponse = await _pharmacyRepository.GetPharmacyById(id);
                if (pharmacyResponse == null)
                {
                    _logger.LogWarning("Pharmacy with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Pharmacy not found.");
                }

                Pharmacy pharmacy = mapper.Map<Pharmacy>(pharmacyResponse);
                var deletedpharmacy = await _pharmacyRepository.DeletePharmacyAsync(pharmacy);
                return deletedpharmacy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Pharmacy with ID: {Id}", id);
                throw;
            }
        }

        public Task<IEnumerable<PharmacyResponse>> GetAllPharmaciesAsync()
        {
            try
            {
                var pharmacy = _pharmacyRepository.GetAllPharmacyAsync();
                return pharmacy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all pharmacies.");
                throw;
            }
        }

        public async Task<PharmacyResponse> GetPharmacyByIdAsync(int id)
        {
            try
            {
                var pharmacy = await _pharmacyRepository.GetPharmacyById(id);
                if (pharmacy == null)
                {
                    _logger.LogWarning("pharmacy with ID: {Id} not found.", id);
                    throw new NotFoundException("pharmacy not found.");
                }

                return pharmacy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching pharmacy with ID: {Id}", id);
                throw;
            }
        }

        public async Task<PharmacyResponse> UpdatePharmacyAsync(int id, PharmacyRequest pharmacyRequest)
        {
            try
            {
                // Fetch the existing building
                var existingpharmacy = await _pharmacyRepository.GetPharmacyById(id);
                if (existingpharmacy == null)
                {
                    _logger.LogWarning("pharmacy with ID: {Id} not found for update.", id);
                    throw new NotFoundException("pharmacy not found.");
                }

                Pharmacy pharmacy = mapper.Map<Pharmacy>(existingpharmacy);

                mapper.Map(pharmacyRequest, pharmacy);

                // Save the updated building
                var updatedpharmacy = await _pharmacyRepository.UpdatePharmacyAsync(id, pharmacy);
                return updatedpharmacy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating pharmacy with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<MedicineResponse>> GetMedicinesByPharmacyIdAsync(int id)
        {
            try
            {
                var pharmacy = await _pharmacyRepository.GetPharmacyById(id);
                if (pharmacy == null)
                {
                    _logger.LogWarning("Pharmacy with ID: {Id} not found.", id);
                    throw new NotFoundException("Pharmacy not found.");
                }

                var medicines = await _medicineRepository.GetMedicinesByPharmacyIdAsync(id);
                return medicines;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching medicines for PharmacyId: {Id}", id);
                throw;
            }
        }
    }
}