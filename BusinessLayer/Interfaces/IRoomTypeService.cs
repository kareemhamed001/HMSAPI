using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IRoomTypeService
    {
        Task<IEnumerable<RoomTypeResponse>> GetAllRoomTypesAsync();
        Task<RoomTypeResponse> GetRoomTypeByIdAsync(int id);
        Task<RoomTypeResponse> CreateRoomTypeAsync(RoomTypeRequest roomTypeRequest);
        Task<RoomTypeResponse> UpdateRoomTypeAsync(int id, RoomTypeRequest roomTypeRequest);
        Task<RoomType> DeleteRoomTypeAsync(int id);
    }
}
