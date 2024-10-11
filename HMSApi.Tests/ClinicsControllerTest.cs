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
using BusinessLayer.Helpers;
using DataAccessLayer.Entities;

namespace LMSApi.Tests
{
    public class ClinicsControllerTest
    {
        private readonly Mock<IClinicService> _clinicServiceMock;
        private readonly Mock<ILogger<ClinicsController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly ClinicsController _controller;

        public ClinicsControllerTest()
        {
            _clinicServiceMock = new Mock<IClinicService>();
            _loggerMock = new Mock<ILogger<ClinicsController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new ClinicsController(_clinicServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllClinics_WhenClinicsExist_ShouldReturnOk()
        {
            // Arrange
            var clinics = new List<ClinicResponse>
            {
                new ClinicResponse { Id = 1, Name = "Clinic A" },
                new ClinicResponse { Id = 2, Name = "Clinic B" }
            };

            _clinicServiceMock.Setup(service => service.GetAllClinicsAsync()).ReturnsAsync(clinics);

            // Act
            var result = await _controller.GetAllClinics();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<ClinicResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Clinics fetched successfully", apiResponse.Message);
            Assert.Equal(clinics, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllClinics_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _clinicServiceMock.Setup(service => service.GetAllClinicsAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllClinics();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetClinicById_WhenClinicExists_ShouldReturnOk()
        {
            // Arrange
            var clinicId = 1;
            var clinic = new ClinicResponse { Id = clinicId, Name = "Clinic A" };

            _clinicServiceMock.Setup(service => service.GetClinicByIdAsync(clinicId)).ReturnsAsync(clinic);

            // Act
            var result = await _controller.GetClinicById(clinicId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Clinic fetched successfully", apiResponse.Message);
            Assert.Equal(clinic, apiResponse.Data);
        }

        [Fact]
        public async Task GetClinicById_WhenClinicDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var clinicId = 1;
            _clinicServiceMock.Setup(service => service.GetClinicByIdAsync(clinicId)).ThrowsAsync(new NotFoundException("Clinic not found"));

            // Act
            var result = await _controller.GetClinicById(clinicId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Clinic not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenClinicIsValid_ShouldReturnOk()
        {
            // Arrange
            var clinicRequest = new ClinicRequest { Name = "Clinic A" };
            var addedClinic = new ClinicResponse { Id = 1, Name = "Clinic A" };

            _clinicServiceMock.Setup(service => service.CreateClinicAsync(clinicRequest)).ReturnsAsync(addedClinic);

            // Act
            var result = await _controller.Add(clinicRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Clinic added successfully", apiResponse.Message);
            Assert.Equal(addedClinic, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var clinicRequest = new ClinicRequest { Name = "Clinic A" };
            _clinicServiceMock.Setup(service => service.CreateClinicAsync(clinicRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(clinicRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenClinicExists_ShouldReturnOk()
        {
            // Arrange
            var clinicId = 1;
            var clinicRequest = new ClinicRequest { Name = "Updated Clinic" };
            var updatedClinic = new ClinicResponse { Id = clinicId, Name = "Updated Clinic" };

            _clinicServiceMock.Setup(service => service.UpdateClinicAsync(clinicId, clinicRequest)).ReturnsAsync(updatedClinic);

            // Act
            var result = await _controller.Update(clinicId, clinicRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Clinic updated successfully", apiResponse.Message);
            Assert.Equal(updatedClinic, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenClinicDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var clinicId = 1;
            var clinicRequest = new ClinicRequest { Name = "Updated Clinic" };
            _clinicServiceMock.Setup(service => service.UpdateClinicAsync(clinicId, clinicRequest)).ThrowsAsync(new NotFoundException("Clinic not found"));

            // Act
            var result = await _controller.Update(clinicId, clinicRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Clinic not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var clinicId = 1;
            var clinicRequest = new ClinicRequest { Name = "Updated Clinic" };
            _clinicServiceMock.Setup(service => service.UpdateClinicAsync(clinicId, clinicRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(clinicId, clinicRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenClinicExists_ShouldReturnOk()
        {
            // Arrange
            var clinicId = 1;
            var deletedClinic = new Clinic { Id = clinicId, Name = "Clinic A" };

            _clinicServiceMock.Setup(service => service.DeleteClinicAsync(clinicId)).ReturnsAsync(deletedClinic);

            // Act
            var result = await _controller.Delete(clinicId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Clinic deleted successfully", apiResponse.Message);
            Assert.Equal(deletedClinic, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenClinicDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var clinicId = 1;
            _clinicServiceMock.Setup(service => service.DeleteClinicAsync(clinicId)).ThrowsAsync(new NotFoundException("Clinic not found"));

            // Act
            var result = await _controller.Delete(clinicId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Clinic not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var clinicId = 1;
            _clinicServiceMock.Setup(service => service.DeleteClinicAsync(clinicId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(clinicId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
