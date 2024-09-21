using DataAccessLayer.Entities;
using SharedClasses.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly AppDbContext _context;

        public StaffRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StaffResponse> CreateStaffAsync(Staff staff)
        {
            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();
            return new StaffResponse
            {
                Id = staff.Id,
                Position = staff.Position,
                Experience = staff.Experience,
                UserId = staff.UserId,
            };
        }

        public async Task<Staff> DeleteStaffAsync(Staff staff)
        {
            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task<IEnumerable<StaffResponse>> GetAllStaffAsync()
        {
            return await _context.Staffs
                .Select(s => new StaffResponse
                {
                    Id = s.Id,
                    Position = s.Position,
                    Experience = s.Experience,
                    UserId = s.UserId,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<StaffResponse?> GetStaffByIdAsync(int id)
        {
            return await _context.Staffs.AsNoTracking()
                .Select(s => new StaffResponse
                {
                    Id = s.Id,
                    Position = s.Position,
                    Experience = s.Experience,
                    UserId = s.UserId,
                })
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<StaffResponse> UpdateStaffAsync(int id, Staff staff)
        {
            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();
            return new StaffResponse
            {
                Id = staff.Id,
                Position = staff.Position,
                Experience = staff.Experience,
                UserId = staff.UserId,
            };
        }
    }
}
