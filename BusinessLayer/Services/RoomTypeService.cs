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
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly ILogger<RoomTypeService> _logger;
        private readonly IMapper mapper;
        public RoomTypeService(IRoomTypeRepository roomRepository, ILogger<RoomTypeService> logger, IMapper mapper)
        {
            _roomTypeRepository = roomRepository;
            _logger = logger;
            this.mapper = mapper;
        }
        public async Task<RoomTypeResponse> CreateRoomTypeAsync(RoomTypeRequest roomTypeRequest)
        {
            try
            {
                RoomType roomType = mapper.Map<RoomType>(roomTypeRequest);

                var createdroomType = await _roomTypeRepository.CreateRoomTypeAsync(roomType);
                return createdroomType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating roomType with Name: {Name}", roomTypeRequest.Name);
                throw;
            }
        }

        public async Task<RoomType> DeleteRoomTypeAsync(int id)
        {
            try
            {
                var roomtypeResponse = await _roomTypeRepository.GetRoomTypeById(id);
                if (roomtypeResponse == null)
                {
                    _logger.LogWarning("roomtype with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("roomtype not found.");
                }
                RoomType roomtype = mapper.Map<RoomType>(roomtypeResponse);
                var deletedRoomType = await _roomTypeRepository.DeleteBuildingAsync(roomtype);
                return deletedRoomType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting RoomType with ID: {Id}", id);
                throw;
            }
        }

        public Task<IEnumerable<RoomTypeResponse>> GetAllRoomTypesAsync()
        {
            try
            {
                var roomTypes = _roomTypeRepository.GetAllRoomTypeAsync();
                return roomTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all roomTypes.");
                throw;
            }
        }

        public async Task<RoomTypeResponse> GetRoomTypeByIdAsync(int id)
        {
            try
            {
                var roomType = await _roomTypeRepository.GetRoomTypeById(id);
                if (roomType == null)
                {
                    _logger.LogWarning("roomType with ID: {Id} not found.", id);
                    throw new NotFoundException("roomType not found.");
                }
                return roomType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching roomType with ID: {Id}", id);
                throw;
            }
        }

        public async Task<RoomTypeResponse> UpdateRoomTypeAsync(int id, RoomTypeRequest roomTypeRequest)
        {
            try
            {
                // Fetch the existing building
                var existingroomType = await _roomTypeRepository.GetRoomTypeById(id);
                if (existingroomType == null)
                {
                    _logger.LogWarning("RoomType with ID: {Id} not found for update.", id);
                    throw new NotFoundException("RoomType not found.");
                }

                RoomType roomType = mapper.Map<RoomType>(existingroomType);

                mapper.Map(roomTypeRequest, roomType);

                // Save the updated building
                var updatedRoomType = await _roomTypeRepository.UpdateBuildingAsync(id, roomType);
                return updatedRoomType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating RoomType with ID: {Id}", id);
                throw;
            }
        }
    }
}
