using AutoMapper;
using BusinessLayer.Requests;
using BusinessLayer.Interfaces;
using BusinessLayer.Responses;
using LMSApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SharedClasses.Exceptions;
using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using BusinessLayer.Helpers;
using DataAccessLayer.Entities;

namespace LMSApi.Tests
{
    public class EmployeeControllerTest
    {
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<ILogger<EmployeeController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly EmployeeController _controller;

        public EmployeeControllerTest()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _loggerMock = new Mock<ILogger<EmployeeController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new EmployeeController(_employeeServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllEmployees_WhenEmployeesExist_ShouldReturnOk()
        {
            // Arrange
            var employees = new List<EmployeeResponse>
            {
                new EmployeeResponse  { SpecializationId =  1 },
                new EmployeeResponse  { SpecializationId =  1 }
            };

            _employeeServiceMock.Setup(service => service.GetAllEmployeesAsync()).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<EmployeeResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Employees fetched successfully", apiResponse.Message);
            Assert.Equal(employees, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllEmployees_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _employeeServiceMock.Setup(service => service.GetAllEmployeesAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetEmployeeById_WhenEmployeeExists_ShouldReturnOk()
        {
            // Arrange
            var employeeId = 1;
            var employee = new EmployeeResponse { SpecializationId = 1 };

            _employeeServiceMock.Setup(service => service.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(employee);

            // Act
            var result = await _controller.GetEmployeeById(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Employee fetched successfully", apiResponse.Message);
            Assert.Equal(employee, apiResponse.Data);
        }

        [Fact]
        public async Task GetEmployeeById_WhenEmployeeDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var employeeId = 1;
            _employeeServiceMock.Setup(service => service.GetEmployeeByIdAsync(employeeId)).ThrowsAsync(new NotFoundException("Employee not found"));

            // Act
            var result = await _controller.GetEmployeeById(employeeId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Employee not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenEmployeeIsValid_ShouldReturnOk()
        {
            // Arrange
            var employeeRequest = new EmployeeRequest { SpecializationId = 1 };
            var addedEmployee = new EmployeeResponse { SpecializationId = 1 };

            _employeeServiceMock.Setup(service => service.CreateEmployeeAsync(employeeRequest)).ReturnsAsync(addedEmployee);

            // Act
            var result = await _controller.Add(employeeRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Employee added successfully", apiResponse.Message);
            Assert.Equal(addedEmployee, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var employeeRequest = new EmployeeRequest { SpecializationId =  1 };
            _employeeServiceMock.Setup(service => service.CreateEmployeeAsync(employeeRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(employeeRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenEmployeeExists_ShouldReturnOk()
        {
            // Arrange
            var employeeId = 1;
            var employeeRequest = new EmployeeRequest { SpecializationId = 1 };
            var updatedEmployee = new EmployeeResponse { SpecializationId = 1 };

            _employeeServiceMock.Setup(service => service.UpdateEmployeeAsync(employeeId, employeeRequest)).ReturnsAsync(updatedEmployee);

            // Act
            var result = await _controller.Update(employeeId, employeeRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Employee updated successfully", apiResponse.Message);
            Assert.Equal(updatedEmployee, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenEmployeeDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var employeeId = 1;
            var employeeRequest = new EmployeeRequest { SpecializationId = 1 };
            _employeeServiceMock.Setup(service => service.UpdateEmployeeAsync(employeeId, employeeRequest)).ThrowsAsync(new NotFoundException("Employee not found"));

            // Act
            var result = await _controller.Update(employeeId, employeeRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Employee not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var employeeId = 1;
            var employeeRequest = new EmployeeRequest { SpecializationId = 1 };
            _employeeServiceMock.Setup(service => service.UpdateEmployeeAsync(employeeId, employeeRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(employeeId, employeeRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenEmployeeExists_ShouldReturnOk()
        {
            // Arrange
            var employeeId = 1;
            var deletedEmployee = new Employee{ Id = employeeId,  };

            _employeeServiceMock.Setup(service => service.DeleteEmployeeAsync(employeeId)).ReturnsAsync(deletedEmployee);

            // Act
            var result = await _controller.Delete(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Employee deleted successfully", apiResponse.Message);
            Assert.Equal(deletedEmployee, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenEmployeeDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var employeeId = 1;
            _employeeServiceMock.Setup(service => service.DeleteEmployeeAsync(employeeId)).ThrowsAsync(new NotFoundException("Employee not found"));

            // Act
            var result = await _controller.Delete(employeeId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Employee not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var employeeId = 1;
            _employeeServiceMock.Setup(service => service.DeleteEmployeeAsync(employeeId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(employeeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
