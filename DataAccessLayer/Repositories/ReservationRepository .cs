using DataAccessLayer.Entities;
using SharedClasses.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReservationResponse> CreateReservationAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return new ReservationResponse
            {
                Id = reservation.Id,
                PatientId = reservation.PatientId,
                DoctorId = reservation.DoctorId,
                ClinicId = reservation.ClinicId,
            };
        }

        public async Task<Reservation> DeleteReservationAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<IEnumerable<ReservationResponse>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .Select(r => new ReservationResponse
                {
                    Id = r.Id,
                    PatientId = r.PatientId,
                    DoctorId = r.DoctorId,
                    ClinicId = r.ClinicId,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ReservationResponse?> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations.AsNoTracking()
                .Select(r => new ReservationResponse
                {
                    Id = r.Id,
                    PatientId = r.PatientId,
                    DoctorId = r.DoctorId,
                    ClinicId = r.ClinicId,
                })
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ReservationResponse> UpdateReservationAsync(int id, Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            return new ReservationResponse
            {
                Id = reservation.Id,
                PatientId = reservation.PatientId,
                DoctorId = reservation.DoctorId,
                ClinicId = reservation.ClinicId,
            };
        }
    }
}
