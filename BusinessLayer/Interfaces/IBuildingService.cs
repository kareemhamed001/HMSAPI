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
        public Task<IEnumerable<BuildingResponse>> GetAllBuildingsAsync(int page,int PerPage);
        Task<BuildingResponse> GetBuildingByIdAsync(int id);
        Task<BuildingResponse> CreateBuildingAsync(BuildingRequest buildingRequest);
        Task<BuildingResponse> UpdateBuildingAsync(int id,BuildingRequest buildingRequest);
        Task<Building> DeleteBuildingAsync(int id);
        Task<IEnumerable<FloorResponse>> GetFloorsByBuildingIdAsync(int buildingId);
    }
}
