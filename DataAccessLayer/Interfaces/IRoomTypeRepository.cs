using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IRoomTypeRepository
    {
        Task<IEnumerable<RoomTypeResponse>> GetAllRoomTypeAsync();
        Task<RoomTypeResponse?> GetRoomTypeById(int id);
        Task<RoomTypeResponse> CreateRoomTypeAsync(RoomType roomType);
        Task<RoomTypeResponse> UpdateBuildingAsync(int id, RoomType roomType);
        Task<RoomType> DeleteBuildingAsync(RoomType roomType);
        Task<IEnumerable<RoomResponse>> GetRoomsByRoomTypeIdAsync(int roomTypeId);
    }
}
