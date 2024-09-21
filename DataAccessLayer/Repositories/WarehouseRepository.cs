using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using SharedClasses.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly AppDbContext _context;

        public WarehouseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WarehouseResponse> CreateWarehouseAsync(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();
            return new WarehouseResponse
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                RoomId = warehouse.RoomId,
                SectionId = warehouse.SectionId
            };
        }

        public async Task<Warehouse> DeleteWarehouseAsync(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse != null)
            {
                _context.Warehouses.Remove(warehouse);
                await _context.SaveChangesAsync();
                return warehouse;
            }
            return null;
        }

        public async Task<IEnumerable<WarehouseResponse>> GetAllWarehousesAsync()
        {
            return await _context.Warehouses
                .Select(w => new WarehouseResponse
                {
                    Id = w.Id,
                    Name = w.Name,
                    RoomId = w.RoomId,
                    SectionId = w.SectionId
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<WarehouseResponse?> GetWarehouseByIdAsync(int id)
        {
            return await _context.Warehouses
                .Where(w => w.Id == id)
                .Select(w => new WarehouseResponse
                {
                    Id = w.Id,
                    Name = w.Name,
                    RoomId = w.RoomId,
                    SectionId = w.SectionId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<WarehouseResponse> UpdateWarehouseAsync(int id, Warehouse warehouse)
        {
            _context.Warehouses.Update(warehouse);
            await _context.SaveChangesAsync();
            return new WarehouseResponse
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                RoomId = warehouse.RoomId,
                SectionId = warehouse.SectionId
            };
        }
    }
}
