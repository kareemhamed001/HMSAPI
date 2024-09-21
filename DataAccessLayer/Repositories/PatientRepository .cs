using DataAccessLayer.Entities;
using SharedClasses.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PatientResponse> CreatePatientAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return new PatientResponse
            {
                Id = patient.Id,
                UserId = patient.UserId,
            };
        }

        public async Task<Patient> DeletePatientAsync(Patient patient)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<IEnumerable<PatientResponse>> GetAllPatientsAsync()
        {
            return await _context.Patients
                .Select(p => new PatientResponse
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Reservations = p.Reservations.Select(r => new ReservationResponse
                    {
                        Id = r.Id,
                        PatientId = r.PatientId,
                        DoctorId = r.DoctorId,
                        ClinicId = r.ClinicId
                    }).ToList(),
                    Prescriptions = p.Prescriptions.Select(pres => new PrescriptionResponse
                    {
                        Id = pres.Id,
                        DoctorId = pres.DoctorId,
                        PatientId = pres.PatientId
                    }).ToList(),
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PatientResponse?> GetPatientById(int id)
        {
            return await _context.Patients
                .AsNoTracking()
                .Select(p => new PatientResponse
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Reservations = p.Reservations.Select(r => new ReservationResponse
                    {
                        Id = r.Id,
                        PatientId = r.PatientId,
                        DoctorId = r.DoctorId,
                        ClinicId = r.ClinicId
                    }).ToList(),
                    Prescriptions = p.Prescriptions.Select(pres => new PrescriptionResponse
                    {
                        Id = pres.Id,
                        DoctorId = pres.DoctorId,
                        PatientId = pres.PatientId
                    }).ToList(),
                })
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PatientResponse> UpdatePatientAsync(int id, Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
            return new PatientResponse
            {
                Id = patient.Id,
                UserId = patient.UserId,
                Reservations = patient.Reservations.Select(r => new ReservationResponse
                {
                    Id = r.Id,
                    PatientId = r.PatientId,
                    DoctorId = r.DoctorId,
                    ClinicId = r.ClinicId
                }).ToList(),
                Prescriptions = patient.Prescriptions.Select(pres => new PrescriptionResponse
                {
                    Id = pres.Id,
                    DoctorId = pres.DoctorId,
                    PatientId = pres.PatientId
                }).ToList(),
            };
        }
    }
}
