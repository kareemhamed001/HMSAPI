using DataAccessLayer.Entities;
using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;
        public RoomRepository(AppDbContext context)
        {
            _context = context;

        }

        public async Task<RoomResponse> CreateRoomAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            RoomResponse response = new RoomResponse
            {
                Id = room.Id,
                Name = room.Name,
                RoomsTypesTd=room.RoomsTypesTd,
                FloorId=room.FloorId,
            };
            return response;
        }

        public async Task<Room> DeleteRoomAsync(Room room)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<IEnumerable<RoomResponse>> GetAllRoomAsync()
        {
            var roomType = await _context.Rooms
             .Select(br => new RoomResponse
             {
                Id = br.Id,
                Name = br.Name,
                 RoomsTypesTd = br.RoomsTypesTd,
                 FloorId = br.FloorId,
             })
             .AsNoTracking()
             .ToListAsync();

            return roomType;
        }

        public async Task<RoomResponse?> GetRoomById(int id)
        {
            return await _context.Rooms.AsNoTracking()
        .Select(br => new RoomResponse
        {
            Id = br.Id,
            Name = br.Name,
            RoomsTypesTd = br.RoomsTypesTd,
            FloorId = br.FloorId,
        })
       .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<RoomResponse> UpdateRoomAsync(int id, Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            RoomResponse response = new RoomResponse
            {
                Id = room.Id,
                Name = room.Name,
                RoomsTypesTd = room.RoomsTypesTd,
                FloorId = room.FloorId,
            };
            return response;
        }

        public async Task<RoomTypeResponse?> GetRoomTypeByRoomIdAsync(int roomId)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room?.RoomType == null)
            {
                return null;
            }

            return new RoomTypeResponse
            {
                Id = room.RoomType.Id,
                Name = room.RoomType.Name,
            };
        }
        
        public async Task<bool> RoomIsAvailableAsync(int id)
        {
            var pharmacy = await _context.pharmacies.FirstOrDefaultAsync(p => p.RoomId == id);
            if (pharmacy is not null)
            {
                return false;
            }

            var clinic = await _context.Clinics.FirstOrDefaultAsync(p => p.RoomId == id);
            if (clinic is not null)
            {
                return false;
            }

            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(p => p.RoomId == id);
            if (warehouse is not null)
            {
                return false;
            }

            return true;
        }
    }
}
