using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IBuildingService
    {
        Task<IEnumerable<BuildingResponse>> GetAllBuildingsAsync();
        Task<BuildingResponse> GetBuildingByIdAsync(int id);
        Task<BuildingResponse> CreateBuildingAsync(BuildingRequest buildingRequest);
        Task<BuildingResponse> UpdateBuildingAsync(int id,BuildingRequest buildingRequest);
        Task<Building> DeleteBuildingAsync(int id);
        Task<IEnumerable<Floor>> GetFloorsByBuildingIdAsync(int buildingId);
    }
}
