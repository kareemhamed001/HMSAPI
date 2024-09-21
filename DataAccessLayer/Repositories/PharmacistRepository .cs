using DataAccessLayer.Entities;
using SharedClasses.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class PharmacistRepository : IPharmacistRepository
    {
        private readonly AppDbContext _context;

        public PharmacistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PharmacistResponse> CreatePharmacistAsync(Pharmacist pharmacist)
        {
            _context.Pharmacists.Add(pharmacist);
            await _context.SaveChangesAsync();
            return new PharmacistResponse
            {
                Id = pharmacist.Id,
                SpecializationId = pharmacist.SpecializationId,
                UserId = pharmacist.UserId,
            };
        }

        public async Task<Pharmacist> DeletePharmacistAsync(Pharmacist pharmacist)
        {
            _context.Pharmacists.Remove(pharmacist);
            await _context.SaveChangesAsync();
            return pharmacist;
        }

        public async Task<IEnumerable<PharmacistResponse>> GetAllPharmacistsAsync()
        {
            return await _context.Pharmacists
                .Select(p => new PharmacistResponse
                {
                    Id = p.Id,
                    SpecializationId = p.SpecializationId,
                    UserId = p.UserId,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PharmacistResponse?> GetPharmacistByIdAsync(int id)
        {
            return await _context.Pharmacists.AsNoTracking()
                .Select(p => new PharmacistResponse
                {
                    Id = p.Id,
                    SpecializationId = p.SpecializationId,
                    UserId = p.UserId,
                })
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PharmacistResponse> UpdatePharmacistAsync(int id, Pharmacist pharmacist)
        {
            _context.Pharmacists.Update(pharmacist);
            await _context.SaveChangesAsync();
            return new PharmacistResponse
            {
                Id = pharmacist.Id,
                SpecializationId = pharmacist.SpecializationId,
                UserId = pharmacist.UserId,
            };
        }
    }
}
