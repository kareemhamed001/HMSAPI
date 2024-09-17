using AutoMapper;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Logging;
using SharedClasses.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class BuildingService: IBuildingService
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly ILogger<BuildingService> _logger;
        private readonly IMapper mapper;
        public BuildingService(IBuildingRepository buildingRepository, ILogger<BuildingService> logger, IMapper mapper)
        {
            _buildingRepository = buildingRepository;
            _logger = logger;
            mapper = mapper;
        }
        public async Task<IEnumerable<Building>> GetAllBuildingsAsync()
        {
            try
            {
                var buildings = await _buildingRepository.GetAllBuildingsAsync();
                return buildings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all buildings.");
                throw;
            }
        }

        public async Task<Building> GetBuildingByIdAsync(int id)
        {
            try
            {
                var building = await _buildingRepository.GetBuildingById(id);
                if (building == null)
                {
                    _logger.LogWarning("Building with ID: {Id} not found.", id);
                    throw new NotFoundException("Building not found.");
                }
                return building;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching building with ID: {Id}", id);
                throw;
            }
        }

        public async Task<Building> CreateBuildingAsync(BuildingRequest buildingRequest)
        {
            try
            {
                Building building= mapper.Map<Building>(buildingRequest);
                var createdBuilding = await _buildingRepository.CreateBuildingAsync(building);
                return createdBuilding;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating building with Name: {Name}", buildingRequest.Name);
                throw;
            }
        }

        public async Task<Building> UpdateBuildingAsync(BuildingRequest buildingRequest)
        {
            if (buildingRequest == null)
            {
                _logger.LogWarning("Invalid building object or ID provided for update.");
                throw new ArgumentException("Invalid building object or ID.");
            }

            try
            {
                Building building = mapper.Map<Building>(buildingRequest);
                var existingBuilding = await _buildingRepository.GetBuildingById(building.Id);
                if (existingBuilding == null)
                {
                    _logger.LogWarning("Building with ID: {Id} not found for update.", building.Id);
                    throw new KeyNotFoundException("Building not found.");
                }
                var updatedBuilding = await _buildingRepository.UpdateBuildingAsync(building);
                return updatedBuilding;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating building with Name: {Name}", buildingRequest.Name);
                throw;
            }
        }

        public async Task<Building> DeleteBuildingAsync(int id)
        {
            try
            {
                var building = await _buildingRepository.GetBuildingById(id);
                if (building == null)
                {
                    _logger.LogWarning("Building with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Building not found.");
                }
                var deletedBuilding = await _buildingRepository.DeleteBuildingAsync(building);
                return deletedBuilding;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting building with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Floor>> GetFloorsByBuildingIdAsync(int buildingId)
        {
            try
            {
                var buildingExists = await _buildingRepository.GetBuildingById(buildingId) != null;

                if (!buildingExists)
                {
                    _logger.LogWarning("Building with ID: {BuildingId} does not exist. Operation took {ElapsedMs} ms.", buildingId);
                    throw new NotFoundException($"Building with ID: {buildingId} does not exist.");
                }
                var floors = await _buildingRepository.GetFloorsByBuildingId(buildingId);
                if (floors == null || !floors.Any())
                {
                    _logger.LogWarning("No floors found for building with ID: {BuildingId}", buildingId);
                    throw new NotFoundException("floors not found.");
                }
                return floors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching floors for building with ID: {BuildingId}", buildingId);
                throw;
            }
        }
    }
}
