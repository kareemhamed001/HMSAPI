using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync();
        Task<EmployeeResponse?> GetEmployeeByIdAsync(int id);
        Task<EmployeeResponse> CreateEmployeeAsync(Employee employee);
        Task<EmployeeResponse> UpdateEmployeeAsync(int id, Employee employee);
        Task<Employee> DeleteEmployeeAsync(Employee employee);
    }
}
