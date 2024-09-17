using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IBuildingRepository
    {
       Task<IEnumerable<Building>> GetAllBuildingsAsync();
        Task<Building> GetBuildingById(int id);
        Task<Building> CreateBuildingAsync(Building building);
        Task<Building> UpdateBuildingAsync(Building building);
        Task<Building> DeleteBuildingAsync(Building building);

        // Additional methods
        Task<IEnumerable<Floor>> GetFloorsByBuildingId(int buildingId);
    }
}
