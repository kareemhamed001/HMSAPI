using DataAccessLayer.Entities;
using SharedClasses.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorResponse> CreateDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return new DoctorResponse
            {
                Id = doctor.Id,
                Description = doctor.Description,
                Education = doctor.Education,
                SpecializationId = doctor.SpecializationId,
                UserId = doctor.UserId,
            };
        }

        public async Task<Doctor> DeleteDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<IEnumerable<DoctorResponse>> GetAllDoctorsAsync()
        {
            return await _context.Doctors
                .Select(d => new DoctorResponse
                {
                    Id = d.Id,
                    Description = d.Description,
                    Education = d.Education,
                    SpecializationId = d.SpecializationId,
                    UserId = d.UserId,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<DoctorResponse?> GetDoctorByIdAsync(int id)
        {
            return await _context.Doctors.AsNoTracking()
                .Select(d => new DoctorResponse
                {
                    Id = d.Id,
                    Description = d.Description,
                    Education = d.Education,
                    SpecializationId = d.SpecializationId,
                    UserId = d.UserId,
                })
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<DoctorResponse> UpdateDoctorAsync(int id, Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
            return new DoctorResponse
            {
                Id = doctor.Id,
                Description = doctor.Description,
                Education = doctor.Education,
                SpecializationId = doctor.SpecializationId,
                UserId = doctor.UserId,
            };
        }
    }
}
