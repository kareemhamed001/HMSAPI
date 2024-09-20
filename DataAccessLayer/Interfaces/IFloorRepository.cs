using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IFloorRepository
    {
        Task<IEnumerable<FloorResponse>> GetAllFloorsAsync();
        Task<FloorResponse> GetFloorById(int id);
        Task<FloorResponse> CreateFloorAsync(Floor floor);
        Task<FloorResponse> UpdateFloorAsync(int id, Floor floor);
        Task<Floor> DeleteFloorAsync(Floor floor);

        // Additional methods
        Task<IEnumerable<Room>> GetRoomsByFloorId(int id);
    }
}
