using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IFloorService
    {
        Task<FloorResponse> CreateFloorAsync(FloorRequest floor);
        Task<FloorResponse> GetFloorByIdAsync(int id);
        Task<IEnumerable<FloorResponse>> GetAllFloorsAsync();
        Task<FloorResponse> UpdateFloorAsync(int id, FloorRequest floor);
        Task<Floor> DeleteFloorAsync(int id);
        Task<IEnumerable<Room>> GetRoomsByFloorIdAsync(int id);
        Task<BuildingResponse> GetBuildingByFloorIdAsync(int id);
    }
}
