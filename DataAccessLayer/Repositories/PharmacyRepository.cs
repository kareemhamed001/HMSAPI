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
    public class PharmacyRepository : IPharmacyRepository
    {
        private readonly AppDbContext _context;

        public PharmacyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PharmacyResponse> CreatePharmacyAsync(Pharmacy pharmacy)
        {
            _context.pharmacies.Add(pharmacy);
            await _context.SaveChangesAsync();
            PharmacyResponse response = new PharmacyResponse
            {
                Id = pharmacy.Id,
                Name = pharmacy.Name,
                RoomId = pharmacy.RoomId,
                Description = pharmacy.Description
            };
            return response;
        }

        public async Task<Pharmacy> DeletePharmacyAsync(Pharmacy pharmacy)
        {
            _context.pharmacies.Remove(pharmacy);
            await _context.SaveChangesAsync();
            return pharmacy;
        }

        public async Task<IEnumerable<PharmacyResponse>> GetAllPharmacyAsync()
        {
            var pharmacy = await _context.pharmacies
                .Select(br => new PharmacyResponse
                {
                    Id = br.Id,
                    Name = br.Name,
                    RoomId = br.RoomId,
                    Description = br.Description
                })
                .AsNoTracking()
                .ToListAsync();

            return pharmacy;
        }

        public async Task<PharmacyResponse?> GetPharmacyById(int id)
        {
            return await _context.pharmacies.AsNoTracking()
                .Select(br => new PharmacyResponse
                {
                    Id = br.Id,
                    Name = br.Name,
                    RoomId = br.RoomId,
                    Description = br.Description
                })
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<PharmacyResponse> UpdatePharmacyAsync(int id, Pharmacy pharmacy)
        {
            _context.pharmacies.Update(pharmacy);
            await _context.SaveChangesAsync();
            PharmacyResponse response = new PharmacyResponse
            {
                Id = pharmacy.Id,
                Name = pharmacy.Name,
                RoomId = pharmacy.RoomId,
                Description = pharmacy.Description
            };
            return response;
        }
    }
}