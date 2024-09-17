using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IBuildingService
    {
        Task<IEnumerable<Building>> GetAllBuildingsAsync();
        Task<Building> GetBuildingByIdAsync(int id);
        Task<Building> CreateBuildingAsync(BuildingRequest buildingRequest);
        Task<Building> UpdateBuildingAsync(BuildingRequest buildingRequest);
        Task<Building> DeleteBuildingAsync(int id);
        Task<IEnumerable<Floor>> GetFloorsByBuildingIdAsync(int buildingId);
    }
}
