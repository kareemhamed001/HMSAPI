using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<RoomResponse>> GetAllRoomAsync();
        Task<RoomResponse?> GetRoomById(int id);
        Task<RoomResponse> CreateRoomAsync(Room room);
        Task<RoomResponse> UpdateRoomAsync(int id, Room room);
        Task<Room> DeleteRoomAsync(Room room);
        Task<RoomTypeResponse?> GetRoomTypeByRoomIdAsync(int roomId);
        public Task<bool> RoomIsAvailableAsync(int id);
    }
}
