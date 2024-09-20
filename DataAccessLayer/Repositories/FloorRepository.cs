using DataAccessLayer.Entities;
using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class FloorRepository:IFloorRepository
    {
        private readonly AppDbContext _context;
        public FloorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FloorResponse> CreateFloorAsync(Floor floor)
        {
            _context.Floors.Add(floor);
            await _context.SaveChangesAsync();
            FloorResponse response = new FloorResponse
            {
                Id = floor.Id,
                Name = floor.Name,
            };
            return response;
        }

        public async Task<Floor> DeleteFloorAsync(Floor floor)
        {
            _context.Floors.Remove(floor);
            await _context.SaveChangesAsync();
            return floor;

        }

        public async Task<IEnumerable<FloorResponse>> GetAllFloorsAsync()
        {
           var floors=await _context.Floors.Select(br => new FloorResponse
            {
                Id = br.Id,
                Name = br.Name,
            }).AsNoTracking().ToListAsync();
            return floors;
        }

        public async Task<FloorResponse> GetFloorById(int id)
        {
            var floor = await _context.Floors.Select(fr => new FloorResponse
            {
                Id = fr.Id,
                Name = fr.Name,
            }).FirstOrDefaultAsync();

            return floor;
        }
        public async Task<FloorResponse> UpdateFloorAsync(int id, Floor floor)
        {
            _context.Floors.Update(floor);
            await _context.SaveChangesAsync();
            FloorResponse response = new FloorResponse
            {
                Id = floor.Id,
                Name = floor.Name,
            };
            return response;
        }


        //additional methods
        public async Task<IEnumerable<Room>> GetRoomsByFloorId(int id)
        {
            var floor = await _context.Floors.Include(f => f.Rooms).FirstOrDefaultAsync(b => b.Id == id);
            var room = floor?.Rooms;
            return room! ;
        }
    }
}
