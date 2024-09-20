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
    public class FloorService:IFloorService
    {
        private readonly IFloorRepository _floorRepository;
        private readonly ILogger<FloorService> _logger;
        private readonly IMapper mapper;
        public FloorService(IFloorRepository floorRepository, ILogger<FloorService> logger,IMapper mapper)
        {
            _floorRepository = floorRepository;
            _logger = logger;
            this.mapper = mapper;
        }

        public async Task<FloorResponse> CreateFloorAsync(FloorRequest floorRequest)
        {
            try
            {
                Floor floor = mapper.Map<Floor>(floorRequest);
                var response = await _floorRepository.CreateFloorAsync(floor);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating floor.");
                throw; 
            }
        }

        public async Task<Floor> DeleteFloorAsync(int id)
        {
            try {
                var floorResponse = await _floorRepository.GetFloorById(id);
                if (floorResponse == null)
                {
                    _logger.LogWarning("floor with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("floor not found.");
                }
                Floor floor = mapper.Map<Floor>(floorResponse);
               var daletedFloor= await _floorRepository.DeleteFloorAsync(floor);
                return daletedFloor;
            }
            catch {
                _logger.LogError("Error occurred while deleting floor with ID: {Id}", id);
                throw;
            }
        }

        public Task<IEnumerable<FloorResponse>> GetAllFloorsAsync()
        {
            try { 
                var floors = _floorRepository.GetAllFloorsAsync();
                return floors;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all floors.");
                throw;
            }
}

        public async Task<FloorResponse> GetFloorByIdAsync(int id)
        {
            try {
                var floor = await _floorRepository.GetFloorById(id);
                if (floor == null)
                {
                    _logger.LogWarning("Building with ID: {Id} not found.", id);
                    throw new NotFoundException("Building not found.");
                }
                return floor;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error occurred while fetching building with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Room>> GetRoomsByFloorIdAsync(int id)
        {
            try
            {
                var FloorExists = await _floorRepository.GetFloorById(id) != null;

                if (!FloorExists)
                {
                    _logger.LogWarning("Flooor with ID: {FlooorId} does not exist. Operation took {ElapsedMs} ms.", id);
                    throw new NotFoundException($"Flooor with ID: {id} does not exist.");
                }
                var rooms = await _floorRepository.GetRoomsByFloorId(id);
                if (rooms == null || !rooms.Any())
                {
                    _logger.LogWarning("No rooms found for building with ID: {FlooorId}", id);
                    throw new NotFoundException("No rooms found for This floor ");
                }
                return rooms;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching rooms for building with ID: {FlooorId}", id);
                throw;
            }
        }

        public async Task<FloorResponse> UpdateFloorAsync(int id, FloorRequest floorRequest)
        {
            try
            {
                // Fetch the existing building
                var existingFloor = await _floorRepository.GetFloorById(id);
                if (existingFloor == null)
                {
                    _logger.LogWarning("Building with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Building not found.");
                }

                Floor floor = mapper.Map<Floor>(existingFloor);

                mapper.Map(floorRequest, floor);

                // Save the updated building
                var updatedBuilding = await _floorRepository.UpdateFloorAsync(id, floor);
                return updatedBuilding;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating building with ID: {Id}", id);
                throw;
            }
        }
    }
}
