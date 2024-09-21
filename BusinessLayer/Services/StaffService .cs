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
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly ILogger<StaffService> _logger;
        private readonly IMapper _mapper;

        public StaffService(IStaffRepository staffRepository, ILogger<StaffService> logger, IMapper mapper)
        {
            _staffRepository = staffRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<StaffResponse> CreateStaffAsync(StaffRequest staffRequest)
        {
            try
            {
                Staff staff = _mapper.Map<Staff>(staffRequest);
                return await _staffRepository.CreateStaffAsync(staff);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating staff with Position: {Position}", staffRequest.Position);
                throw;
            }
        }

        public async Task<Staff> DeleteStaffAsync(int id)
        {
            try
            {
                var staffResponse = await _staffRepository.GetStaffByIdAsync(id);
                if (staffResponse == null)
                {
                    _logger.LogWarning("Staff with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Staff not found.");
                }
                Staff staff = _mapper.Map<Staff>(staffResponse);
                return await _staffRepository.DeleteStaffAsync(staff);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting staff with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<StaffResponse>> GetAllStaffAsync()
        {
            try
            {
                return await _staffRepository.GetAllStaffAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all staff.");
                throw;
            }
        }

        public async Task<StaffResponse> GetStaffByIdAsync(int id)
        {
            try
            {
                var staff = await _staffRepository.GetStaffByIdAsync(id);
                if (staff == null)
                {
                    _logger.LogWarning("Staff with ID: {Id} not found.", id);
                    throw new NotFoundException("Staff not found.");
                }
                return staff;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching staff with ID: {Id}", id);
                throw;
            }
        }

        public async Task<StaffResponse> UpdateStaffAsync(int id, StaffRequest staffRequest)
        {
            try
            {
                var existingStaff = await _staffRepository.GetStaffByIdAsync(id);
                if (existingStaff == null)
                {
                    _logger.LogWarning("Staff with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Staff not found.");
                }

                Staff staff = _mapper.Map<Staff>(existingStaff);
                _mapper.Map(staffRequest, staff);
                return await _staffRepository.UpdateStaffAsync(id, staff);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating staff with ID: {Id}", id);
                throw;
            }
        }
    }
}
