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
    public class PatientControllerTest
    {
        private readonly Mock<IPatientService> _patientServiceMock;
        private readonly Mock<ILogger<PatientController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly PatientController _controller;

        public PatientControllerTest()
        {
            _patientServiceMock = new Mock<IPatientService>();
            _loggerMock = new Mock<ILogger<PatientController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new PatientController(_patientServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllPatients_WhenPatientsExist_ShouldReturnOk()
        {
            // Arrange
            var patients = new List<PatientResponse>
            {
                new PatientResponse { UserId = 1,  },
                new PatientResponse { UserId = 2,  }
            };

            _patientServiceMock.Setup(service => service.GetAllPatientsAsync()).ReturnsAsync(patients);

            // Act
            var result = await _controller.GetAllPatients();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<PatientResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Patients fetched successfully", apiResponse.Message);
            Assert.Equal(patients, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllPatients_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _patientServiceMock.Setup(service => service.GetAllPatientsAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllPatients();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetPatientById_WhenPatientExists_ShouldReturnOk()
        {
            // Arrange
            var patientId = 1;
            var patient = new PatientResponse { UserId = 2, };

            _patientServiceMock.Setup(service => service.GetPatientByIdAsync(patientId)).ReturnsAsync(patient);

            // Act
            var result = await _controller.GetPatientById(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Patient fetched successfully", apiResponse.Message);
            Assert.Equal(patient, apiResponse.Data);
        }

        [Fact]
        public async Task GetPatientById_WhenPatientDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var patientId = 1;
            _patientServiceMock.Setup(service => service.GetPatientByIdAsync(patientId)).ThrowsAsync(new NotFoundException("Patient not found"));

            // Act
            var result = await _controller.GetPatientById(patientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Patient not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenPatientIsValid_ShouldReturnOk()
        {
            // Arrange
            var patientRequest = new PatientRequest { UserId = 2, };
            var addedPatient = new PatientResponse { UserId = 2, };

            _patientServiceMock.Setup(service => service.CreatePatientAsync(patientRequest)).ReturnsAsync(addedPatient);

            // Act
            var result = await _controller.Add(patientRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Patient added successfully", apiResponse.Message);
            Assert.Equal(addedPatient, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var patientRequest = new PatientRequest { UserId = 2, };
            _patientServiceMock.Setup(service => service.CreatePatientAsync(patientRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(patientRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenPatientExists_ShouldReturnOk()
        {
            // Arrange
            var patientId = 1;
            var patientRequest = new PatientRequest { UserId = 2, };
            var updatedPatient = new PatientResponse { UserId = 2, };

            _patientServiceMock.Setup(service => service.UpdatePatientAsync(patientId, patientRequest)).ReturnsAsync(updatedPatient);

            // Act
            var result = await _controller.Update(patientId, patientRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Patient updated successfully", apiResponse.Message);
            Assert.Equal(updatedPatient, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenPatientDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var patientId = 1;
            var patientRequest = new PatientRequest { UserId = 2, };
            _patientServiceMock.Setup(service => service.UpdatePatientAsync(patientId, patientRequest)).ThrowsAsync(new NotFoundException("Patient not found"));

            // Act
            var result = await _controller.Update(patientId, patientRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Patient not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var patientId = 1;
            var patientRequest = new PatientRequest { UserId = 2, };
            _patientServiceMock.Setup(service => service.UpdatePatientAsync(patientId, patientRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(patientId, patientRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenPatientExists_ShouldReturnOk()
        {
            // Arrange
            var patientId = 1;
            var deletedPatient = new Patient { UserId = 2, };

            _patientServiceMock.Setup(service => service.DeletePatientAsync(patientId)).ReturnsAsync(deletedPatient);

            // Act
            var result = await _controller.Delete(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Patient deleted successfully", apiResponse.Message);
            Assert.Equal(deletedPatient, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenPatientDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var patientId = 1;
            _patientServiceMock.Setup(service => service.DeletePatientAsync(patientId)).ThrowsAsync(new NotFoundException("Patient not found"));

            // Act
            var result = await _controller.Delete(patientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Patient not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var patientId = 1;
            _patientServiceMock.Setup(service => service.DeletePatientAsync(patientId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(patientId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
