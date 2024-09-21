using DataAccessLayer.Entities;
using SharedClasses.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly AppDbContext _context;

        public PrescriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PrescriptionResponse> CreatePrescriptionAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return new PrescriptionResponse
            {
                Id = prescription.Id,
                DoctorId = prescription.DoctorId,
                PatientId = prescription.PatientId,
            };
        }

        public async Task<Prescription> DeletePrescriptionAsync(Prescription prescription)
        {
            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }

        public async Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync()
        {
            return await _context.Prescriptions
                .Select(p => new PrescriptionResponse
                {
                    Id = p.Id,
                    DoctorId = p.DoctorId,
                    PatientId = p.PatientId,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PrescriptionResponse?> GetPrescriptionByIdAsync(int id)
        {
            return await _context.Prescriptions.AsNoTracking()
                .Select(p => new PrescriptionResponse
                {
                    Id = p.Id,
                    DoctorId = p.DoctorId,
                    PatientId = p.PatientId,
                })
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PrescriptionResponse> UpdatePrescriptionAsync(int id, Prescription prescription)
        {
            _context.Prescriptions.Update(prescription);
            await _context.SaveChangesAsync();
            return new PrescriptionResponse
            {
                Id = prescription.Id,
                DoctorId = prescription.DoctorId,
                PatientId = prescription.PatientId,
            };
        }
    }
}
