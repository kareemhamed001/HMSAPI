using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedClasses.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ClinicRepository : IClinicRepository
    {
        private readonly AppDbContext _context;

        public ClinicRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ClinicResponse> CreateClinicAsync(Clinic clinic)
        {
            _context.Clinics.Add(clinic);
            await _context.SaveChangesAsync();
            return new ClinicResponse
            {
                Id = clinic.Id,
                Name = clinic.Name,
                SectionId = clinic.SectionId,
                ReservationIds = clinic.Reservations.Select(r => r.Id).ToList(),
            };
        }

        public async Task<Clinic> DeleteClinicAsync(int id)
        {
            var clinic = await _context.Clinics
                .Include(c => c.Reservations)
                .Include(c => c.ClinicDoctors)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (clinic != null)
            {
                _context.Clinics.Remove(clinic);
                await _context.SaveChangesAsync();
                return clinic;
            }
            return null;
        }

        public async Task<IEnumerable<ClinicResponse>> GetAllClinicsAsync()
        {
            return await _context.Clinics
                .Select(c => new ClinicResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    SectionId = c.SectionId,
                    ReservationIds = c.Reservations.Select(r => r.Id).ToList(),
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ClinicResponse?> GetClinicByIdAsync(int id)
        {
            return await _context.Clinics
                .Where(c => c.Id == id)
                .Select(c => new ClinicResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    SectionId = c.SectionId,
                    ReservationIds = c.Reservations.Select(r => r.Id).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ClinicResponse> UpdateClinicAsync(int id, Clinic clinic)
        {
            _context.Clinics.Update(clinic);
            await _context.SaveChangesAsync();
            return new ClinicResponse
            {
                Id = clinic.Id,
                Name = clinic.Name,
                SectionId = clinic.SectionId,
                ReservationIds = clinic.Reservations.Select(r => r.Id).ToList(),
            };
        }
    }
}
