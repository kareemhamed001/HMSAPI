using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using SharedClasses.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _context;

        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SupplierResponse> CreateSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return new SupplierResponse
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Description = supplier.Description,
            };
        }

        public async Task<Supplier> DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.Include(s => s.Medicines).FirstOrDefaultAsync(s => s.Id == id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
                return supplier;
            }
            return null;
        }

        public async Task<IEnumerable<SupplierResponse>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers
                .Select(s => new SupplierResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SupplierResponse?> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers
                .Where(s => s.Id == id)
                .Select(s => new SupplierResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<SupplierResponse> UpdateSupplierAsync(int id, Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
            return new SupplierResponse
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Description = supplier.Description,
            };
        }
        public async Task<IEnumerable<MedicineResponse>> GetMedicinesBySupplierIdAsync(int supplierId)
        {
            var medicines = await _context.Medicines
                .Where(m => m.SupplierId == supplierId) 
                .ToListAsync();

            return medicines.Select(m => new MedicineResponse
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
            }).ToList();
        }
    }
}
