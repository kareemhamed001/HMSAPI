using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedClasses.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class NurseRepository : INurseRepository
    {
        private readonly AppDbContext _context;

        public NurseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<NurseResponse> CreateNurseAsync(Nurse nurse)
        {
            _context.Nurses.Add(nurse);
            await _context.SaveChangesAsync();
            return new NurseResponse
            {
                Id = nurse.Id,
                Passport = nurse.Passport,
                Experience = nurse.Experience,
                Education = nurse.Education,
                AdditionalInfo = nurse.AdditionalInfo,
                SpecializationId = nurse.SpecializationId,
                UserId = nurse.UserId
            };
        }

        public async Task<Nurse> DeleteNurseAsync(int id)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse != null)
            {
                _context.Nurses.Remove(nurse);
                await _context.SaveChangesAsync();
            }
            return nurse;
        }

        public async Task<IEnumerable<NurseResponse>> GetAllNursesAsync()
        {
            return await _context.Nurses
                .Select(n => new NurseResponse
                {
                    Id = n.Id,
                    Passport = n.Passport,
                    Experience = n.Experience,
                    Education = n.Education,
                    AdditionalInfo = n.AdditionalInfo,
                    SpecializationId = n.SpecializationId,
                    UserId = n.UserId
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<NurseResponse?> GetNurseByIdAsync(int id)
        {
            return await _context.Nurses
                .Where(n => n.Id == id)
                .Select(n => new NurseResponse
                {
                    Id = n.Id,
                    Passport = n.Passport,
                    Experience = n.Experience,
                    Education = n.Education,
                    AdditionalInfo = n.AdditionalInfo,
                    SpecializationId = n.SpecializationId,
                    UserId = n.UserId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<NurseResponse> UpdateNurseAsync(int id, Nurse nurse)
        {
            _context.Nurses.Update(nurse);
            await _context.SaveChangesAsync();
            return new NurseResponse
            {
                Id = nurse.Id,
                Passport = nurse.Passport,
                Experience = nurse.Experience,
                Education = nurse.Education,
                AdditionalInfo = nurse.AdditionalInfo,
                SpecializationId = nurse.SpecializationId,
                UserId = nurse.UserId
            };
        }
    }
}
