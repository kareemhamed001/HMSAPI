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
using System.Xml.Linq;

namespace LMSApi.Tests
{
    public class DoctorControllerTest
    {
        private readonly Mock<IDoctorService> _doctorServiceMock;
        private readonly Mock<ILogger<DoctorController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly DoctorController _controller;

        public DoctorControllerTest()
        {
            _doctorServiceMock = new Mock<IDoctorService>();
            _loggerMock = new Mock<ILogger<DoctorController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new DoctorController(_doctorServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllDoctors_WhenDoctorsExist_ShouldReturnOk()
        {
            // Arrange
            var doctors = new List<DoctorResponse>
            {
                new DoctorResponse { Id = 1, Description = "Doctor A" },
                new DoctorResponse { Id = 2, Description = "Doctor B" }
            };

            _doctorServiceMock.Setup(service => service.GetAllDoctorsAsync()).ReturnsAsync(doctors);

            // Act
            var result = await _controller.GetAllDoctors();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<DoctorResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Doctors fetched successfully", apiResponse.Message);
            Assert.Equal(doctors, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllDoctors_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _doctorServiceMock.Setup(service => service.GetAllDoctorsAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllDoctors();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetDoctorById_WhenDoctorExists_ShouldReturnOk()
        {
            // Arrange
            var doctorId = 1;
            var doctor = new DoctorResponse { Id = doctorId, Description = "Doctor A" };

            _doctorServiceMock.Setup(service => service.GetDoctorByIdAsync(doctorId)).ReturnsAsync(doctor);

            // Act
            var result = await _controller.GetDoctorById(doctorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Doctor fetched successfully", apiResponse.Message);
            Assert.Equal(doctor, apiResponse.Data);
        }

        [Fact]
        public async Task GetDoctorById_WhenDoctorDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var doctorId = 1;
            _doctorServiceMock.Setup(service => service.GetDoctorByIdAsync(doctorId)).ThrowsAsync(new NotFoundException("Doctor not found"));

            // Act
            var result = await _controller.GetDoctorById(doctorId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Doctor not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenDoctorIsValid_ShouldReturnOk()
        {
            // Arrange
            var doctorRequest = new DoctorRequest { Description = "Doctor A" };
            var addedDoctor = new DoctorResponse { Id = 1, Description = "Doctor A" };

            _doctorServiceMock.Setup(service => service.CreateDoctorAsync(doctorRequest)).ReturnsAsync(addedDoctor);

            // Act
            var result = await _controller.Add(doctorRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Doctor added successfully", apiResponse.Message);
            Assert.Equal(addedDoctor, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var doctorRequest = new DoctorRequest { Description = "Doctor A" };
            _doctorServiceMock.Setup(service => service.CreateDoctorAsync(doctorRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(doctorRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenDoctorExists_ShouldReturnOk()
        {
            // Arrange
            var doctorId = 1;
            var doctorRequest = new DoctorRequest { Description = "Updated Doctor" };
            var updatedDoctor = new DoctorResponse { Id = doctorId, Description = "Updated Doctor" };

            _doctorServiceMock.Setup(service => service.UpdateDoctorAsync(doctorId, doctorRequest)).ReturnsAsync(updatedDoctor);

            // Act
            var result = await _controller.Update(doctorId, doctorRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Doctor updated successfully", apiResponse.Message);
            Assert.Equal(updatedDoctor, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenDoctorDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var doctorId = 1;
            var doctorRequest = new DoctorRequest { Description = "Updated Doctor" };
            _doctorServiceMock.Setup(service => service.UpdateDoctorAsync(doctorId, doctorRequest)).ThrowsAsync(new NotFoundException("Doctor not found"));

            // Act
            var result = await _controller.Update(doctorId, doctorRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Doctor not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var doctorId = 1;
            var doctorRequest = new DoctorRequest { Description = "Updated Doctor" };
            _doctorServiceMock.Setup(service => service.UpdateDoctorAsync(doctorId, doctorRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(doctorId, doctorRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenDoctorExists_ShouldReturnOk()
        {
            // Arrange
            var doctorId = 1;
            var deletedDoctor = new Doctor { Id = doctorId, Description = "Doctor A" };

            _doctorServiceMock.Setup(service => service.DeleteDoctorAsync(doctorId)).ReturnsAsync(deletedDoctor);

            // Act
            var result = await _controller.Delete(doctorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Doctor deleted successfully", apiResponse.Message);
            Assert.Equal(deletedDoctor, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenDoctorDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var doctorId = 1;
            _doctorServiceMock.Setup(service => service.DeleteDoctorAsync(doctorId)).ThrowsAsync(new NotFoundException("Doctor not found"));

            // Act
            var result = await _controller.Delete(doctorId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Doctor not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var doctorId = 1;
            _doctorServiceMock.Setup(service => service.DeleteDoctorAsync(doctorId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(doctorId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
