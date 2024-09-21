using DataAccessLayer.Entities;
using SharedClasses.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeResponse> CreateEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return new EmployeeResponse
            {
                Id = employee.Id,
                SpecializationId = employee.SpecializationId,
                UserId = employee.UserId,
            };
        }

        public async Task<Employee> DeleteEmployeeAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Select(e => new EmployeeResponse
                {
                    Id = e.Id,
                    SpecializationId = e.SpecializationId,
                    UserId = e.UserId,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<EmployeeResponse?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.AsNoTracking()
                .Select(e => new EmployeeResponse
                {
                    Id = e.Id,
                    SpecializationId = e.SpecializationId,
                    UserId = e.UserId,
                })
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<EmployeeResponse> UpdateEmployeeAsync(int id, Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return new EmployeeResponse
            {
                Id = employee.Id,
                SpecializationId = employee.SpecializationId,
                UserId = employee.UserId,
            };
        }
    }
}
