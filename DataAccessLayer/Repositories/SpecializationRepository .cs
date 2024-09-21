using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using SharedClasses.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly AppDbContext _context;

        public SpecializationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SpecializationResponse> CreateSpecializationAsync(Specialization specialization)
        {
            _context.Specializations.Add(specialization);
            await _context.SaveChangesAsync();
            return new SpecializationResponse
            {
                Id = specialization.Id,
                Name = specialization.Name,
                Description = specialization.Description,
                Type = specialization.Type,
                DoctorIds = specialization.Doctors.Select(d => d.Id).ToList()
            };
        }

        public async Task<Specialization> DeleteSpecializationAsync(int id)
        {
            var specialization = await _context.Specializations
                .Include(s => s.Doctors)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (specialization != null)
            {
                _context.Specializations.Remove(specialization);
                await _context.SaveChangesAsync();
                return specialization;
            }
            return null;
        }

        public async Task<IEnumerable<SpecializationResponse>> GetAllSpecializationsAsync()
        {
            return await _context.Specializations
                .Select(s => new SpecializationResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Type = s.Type,
                    DoctorIds = s.Doctors.Select(d => d.Id).ToList()
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SpecializationResponse?> GetSpecializationByIdAsync(int id)
        {
            return await _context.Specializations
                .Where(s => s.Id == id)
                .Select(s => new SpecializationResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Type = s.Type,
                    DoctorIds = s.Doctors.Select(d => d.Id).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<SpecializationResponse> UpdateSpecializationAsync(int id, Specialization specialization)
        {
            _context.Specializations.Update(specialization);
            await _context.SaveChangesAsync();
            return new SpecializationResponse
            {
                Id = specialization.Id,
                Name = specialization.Name,
                Description = specialization.Description,
                Type = specialization.Type,
                DoctorIds = specialization.Doctors.Select(d => d.Id).ToList()
            };
        }
    }
}
