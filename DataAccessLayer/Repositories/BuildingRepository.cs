using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly AppDbContext _context;
        public BuildingRepository(AppDbContext context)
        {
            _context = context;
          
        }
        public async Task<Building> CreateBuildingAsync(Building building)
        {
          _context.Buildings.Add(building);
            await _context.SaveChangesAsync();
            return building;
        }

        public async Task<Building> DeleteBuildingAsync(Building building)
        {
            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();
            return building;
        }

        public async Task<IEnumerable<Building>> GetAllBuildingsAsync()
        {
            var buildings= await _context.Buildings.ToListAsync();
            return buildings;
        }

        public async Task<Building> GetBuildingById(int id)
        {
            return await _context.Buildings.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Building> UpdateBuildingAsync(Building building)
        {
            _context.Buildings.Update(building);
            await _context.SaveChangesAsync();
            return building;
        }


        // Additional methods
        public async Task<IEnumerable<Floor>> GetFloorsByBuildingId(int buildingId)
        {
            var building=await _context.Buildings.Include(b=>b.Floors).FirstOrDefaultAsync(b => b.Id == buildingId);
            var floors=building.Floors;
            return floors;
        }

       
    }
}
