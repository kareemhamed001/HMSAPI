using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Requests;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Logging;
using SharedClasses.Exceptions;
using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<EmployeeResponse> CreateEmployeeAsync(EmployeeRequest employeeRequest)
        {
            try
            {
                Employee employee = _mapper.Map<Employee>(employeeRequest);
                return await _employeeRepository.CreateEmployeeAsync(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating employee with UserId: {UserId}", employeeRequest.UserId);
                throw;
            }
        }

        public async Task<Employee> DeleteEmployeeAsync(int id)
        {
            try
            {
                var employeeResponse = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (employeeResponse == null)
                {
                    _logger.LogWarning("Employee with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Employee not found.");
                }
                Employee employee = _mapper.Map<Employee>(employeeResponse);
                return await _employeeRepository.DeleteEmployeeAsync(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting employee with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync()
        {
            try
            {
                return await _employeeRepository.GetAllEmployeesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all employees.");
                throw;
            }
        }

        public async Task<EmployeeResponse> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    _logger.LogWarning("Employee with ID: {Id} not found.", id);
                    throw new NotFoundException("Employee not found.");
                }
                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employee with ID: {Id}", id);
                throw;
            }
        }

        public async Task<EmployeeResponse> UpdateEmployeeAsync(int id, EmployeeRequest employeeRequest)
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (existingEmployee == null)
                {
                    _logger.LogWarning("Employee with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Employee not found.");
                }

                Employee employee = _mapper.Map<Employee>(existingEmployee);
                _mapper.Map(employeeRequest, employee);
                return await _employeeRepository.UpdateEmployeeAsync(id, employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating employee with ID: {Id}", id);
                throw;
            }
        }
    }
}
