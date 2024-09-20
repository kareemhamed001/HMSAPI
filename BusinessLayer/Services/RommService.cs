using AutoMapper;
using BusinessLayer.Requests;
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
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<RoomService> _logger;
        private readonly IMapper mapper;
        public RoomService(IRoomRepository roomRepository, ILogger<RoomService> logger, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _logger = logger;
            this.mapper = mapper;
        }
        public async Task<RoomResponse> CreateRoomAsync(RoomRequest roomRequest)
        {
            try
            {
                Room room = mapper.Map<Room>(roomRequest);

                var createdroom = await _roomRepository.CreateRoomAsync(room);
                return createdroom;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating room with Name: {Name}", roomRequest.Name);
                throw;
            }
        }

        public async Task<Room> DeleteRoomAsync(int id)
        {
            try
            {
                var roomResponse = await _roomRepository.GetRoomById(id);
                if (roomResponse == null)
                {
                    _logger.LogWarning("roomtype with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("roomtype not found.");
                }
                Room room = mapper.Map<Room>(roomResponse);
                var deletedRoom = await _roomRepository.DeleteRoomAsync(room);
                return deletedRoom;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting RoomType with ID: {Id}", id);
                throw;
            }
        }

        public Task<IEnumerable<RoomResponse>> GetAllRoomAsync()
        {
            try
            {
                var room = _roomRepository.GetAllRoomAsync();
                return room;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all rooms.");
                throw;
            }
        }

        public async Task<RoomResponse> GetRoomByIdAsync(int id)
        {
            try
            {
                var room = await _roomRepository.GetRoomById(id);
                if (room == null)
                {
                    _logger.LogWarning("room with ID: {Id} not found.", id);
                    throw new NotFoundException("room not found.");
                }
                return room;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching roomType with ID: {Id}", id);
                throw;
            }
        }

        public async Task<RoomResponse> UpdateRoomAsync(int id, RoomRequest roomRequest)
        {
            try
            {
                // Fetch the existing building
                var existingroom = await _roomRepository.GetRoomById(id);
                if (existingroom == null)
                {
                    _logger.LogWarning("Room with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Room not found.");
                }

                Room room = mapper.Map<Room>(existingroom);

                mapper.Map(roomRequest, room);

                // Save the updated building
                var updatedRoom = await _roomRepository.UpdateRoomAsync(id, room);
                return updatedRoom;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Room with ID: {Id}", id);
                throw;
            }
        }
    }
}
