using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly AppDbContext _context;
        public RoomTypeRepository(AppDbContext context)
        {
            _context = context;

        }
        public async Task<RoomTypeResponse> CreateRoomTypeAsync(RoomType roomType)
        {
            _context.RoomTypes.Add(roomType);
            await _context.SaveChangesAsync();
            RoomTypeResponse response = new RoomTypeResponse
            {
                Id = roomType.Id,
                Name = roomType.Name,
            };
            return response;
        }

        public async Task<RoomType> DeleteBuildingAsync(RoomType roomType)
        {
            _context.RoomTypes.Remove(roomType);
            await _context.SaveChangesAsync();
            return roomType;
        }

        public async Task<IEnumerable<RoomTypeResponse>> GetAllRoomTypeAsync()
        {
            var roomType = await _context.RoomTypes
               .Select(br => new RoomTypeResponse
               {
                   Id = br.Id,
                   Name = br.Name,
               })
               .AsNoTracking()
               .ToListAsync();
            return roomType;
        }

        public async Task<RoomTypeResponse?> GetRoomTypeById(int id)
        {
            return await _context.RoomTypes.AsNoTracking()
          .Select(br => new RoomTypeResponse
          {
             Id = br.Id,
             Name = br.Name,
          })
         .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<RoomTypeResponse> UpdateBuildingAsync(int id, RoomType roomType)
        {
            _context.RoomTypes.Update(roomType);
            await _context.SaveChangesAsync();
            RoomTypeResponse response = new RoomTypeResponse
            {
                Id = roomType.Id,
                Name = roomType.Name,
            };
            return response;
        }
        public async Task<IEnumerable<RoomResponse>> GetRoomsByRoomTypeIdAsync(int roomTypeId)
        {
            var rooms = await _context.Rooms
                .Where(r => r.RoomsTypesTd == roomTypeId)
                .Select(r => new RoomResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                    FloorId = r.FloorId,
                    RoomsTypesTd = r.RoomsTypesTd
                })
                .ToListAsync();

            return rooms;
        }
    }
}
