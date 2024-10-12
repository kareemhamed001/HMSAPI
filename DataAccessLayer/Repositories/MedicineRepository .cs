using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using SharedClasses.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly AppDbContext _context;

        public MedicineRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MedicineResponse> CreateMedicineAsync(Medicine medicine)
        {
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();
            return new MedicineResponse
            {
                Id = medicine.Id,
                Name = medicine.Name,
                Description = medicine.Description,
                SupplierId = medicine.SupplierId
            };
        }

        public async Task<Medicine> DeleteMedicineAsync(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine != null)
            {
                _context.Medicines.Remove(medicine);
                await _context.SaveChangesAsync();
                return medicine;
            }
            return null;
        }

        public async Task<IEnumerable<MedicineResponse>> GetAllMedicinesAsync()
        {
            return await _context.Medicines
                .Select(m => new MedicineResponse
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    SupplierId = m.SupplierId
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<MedicineResponse?> GetMedicineByIdAsync(int id)
        {
            return await _context.Medicines
                .Where(m => m.Id == id)
                .Select(m => new MedicineResponse
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    SupplierId = m.SupplierId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<MedicineResponse> UpdateMedicineAsync(int id, Medicine medicine)
        {
            _context.Medicines.Update(medicine);
            await _context.SaveChangesAsync();
            return new MedicineResponse
            {
                Id = medicine.Id,
                Name = medicine.Name,
                Description = medicine.Description,
                SupplierId = medicine.SupplierId
            };
        }

        public async Task<SupplierResponse?> GetSupplierByMedicineIdAsync(int medicineId)
        {
            var medicine = await _context.Medicines
                .Include(m => m.Supplier) 
                .FirstOrDefaultAsync(m => m.Id == medicineId);

            if (medicine?.Supplier == null) return null;

            return new SupplierResponse
            {
                Id = medicine.Supplier.Id,
                Name = medicine.Supplier.Name,
                Description = medicine.Supplier.Description,
            };
        }
    }
}
