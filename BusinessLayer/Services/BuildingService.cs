using AutoMapper;
using BusinessLayer.Responses;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Logging;
using SharedClasses.Exceptions;
using SharedClasses.Responses;

namespace BusinessLayer.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly ILogger<BuildingService> _logger;
        private readonly IMapper mapper;
        public BuildingService(IBuildingRepository buildingRepository, ILogger<BuildingService> logger, IMapper mapper)
        {
            _buildingRepository = buildingRepository;
            _logger = logger;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<BuildingResponse>> GetAllBuildingsAsync()
        {
            try
            {
                var buildings =(List<BuildingResponse>) await _buildingRepository.GetAllBuildingsAsync();
                return buildings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all buildings.");
                throw;
            }
        }

        public async Task<BuildingResponse> GetBuildingByIdAsync(int id)
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

        public async Task<BuildingResponse> CreateBuildingAsync(BuildingRequest buildingRequest)
        {
            try
            {
              
                Building building = mapper.Map<Building>(buildingRequest);
    
                var createdBuilding = await _buildingRepository.CreateBuildingAsync(building);
                return createdBuilding;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating building with Name: {Name}", buildingRequest.Name);
                throw;
            }
        }

        public async Task<BuildingResponse> UpdateBuildingAsync(int id ,BuildingRequest buildingRequest)
        {
            try
            {
                // Fetch the existing building
                var existingBuilding = await _buildingRepository.BuildingExist(id);
                if (existingBuilding == null)
                {
                    _logger.LogWarning("Building with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Building not found.");
                }

                // Map updated properties from the request
                mapper.Map(buildingRequest, existingBuilding);

                // Save the updated building
                var updatedBuilding = await _buildingRepository.UpdateBuildingAsync(existingBuilding);
                return updatedBuilding;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating building with ID: {Id}", id);
                throw;
            }
        }

        public async Task<Building> DeleteBuildingAsync(int id)
        {
            try
            {
                var buildingResponse = await _buildingRepository.GetBuildingById(id);
                if (buildingResponse == null)
                {
                    _logger.LogWarning("Building with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Building not found.");
                }
                Building building=mapper.Map<Building>(buildingResponse);
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
                    throw new NotFoundException("No floors found for This building ");
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
