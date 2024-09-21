using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync();
        Task<EmployeeResponse> GetEmployeeByIdAsync(int id);
        Task<EmployeeResponse> CreateEmployeeAsync(EmployeeRequest employeeRequest);
        Task<EmployeeResponse> UpdateEmployeeAsync(int id, EmployeeRequest employeeRequest);
        Task<Employee> DeleteEmployeeAsync(int id);
    }
}
