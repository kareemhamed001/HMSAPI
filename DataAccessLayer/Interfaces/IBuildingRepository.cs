using SharedClasses.Responses;

namespace DataAccessLayer.Interfaces
{
    public interface IBuildingRepository
    {
       Task<IEnumerable<BuildingResponse>> GetAllBuildingsAsync();
        Task<BuildingResponse?> GetBuildingById(int id);
        Task<BuildingResponse> CreateBuildingAsync(Building building);
        Task<BuildingResponse> UpdateBuildingAsync(Building building);
        Task<Building> DeleteBuildingAsync(Building building);

        // Additional methods
        Task<IEnumerable<Floor>> GetFloorsByBuildingId(int buildingId);
        Task<Building> BuildingExist(int id);
    }
}
