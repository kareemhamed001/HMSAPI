using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomResponse>> GetAllRoomAsync();
        Task<RoomResponse> GetRoomByIdAsync(int id);
        Task<RoomResponse> CreateRoomAsync(RoomRequest roomRequest);
        Task<RoomResponse> UpdateRoomAsync(int id, RoomRequest roomRequest);
        Task<Room> DeleteRoomAsync(int id);
        Task<RoomTypeResponse?> GetRoomTypeByRoomIdAsync(int roomId);
    }
}
