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
    public class PharmacistControllerTest
    {
        private readonly Mock<IPharmacistService> _pharmacistServiceMock;
        private readonly Mock<ILogger<PharmacistController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly PharmacistController _controller;

        public PharmacistControllerTest()
        {
            _pharmacistServiceMock = new Mock<IPharmacistService>();
            _loggerMock = new Mock<ILogger<PharmacistController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new PharmacistController(_pharmacistServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllPharmacists_WhenPharmacistsExist_ShouldReturnOk()
        {
            // Arrange
            var pharmacists = new List<PharmacistResponse>
            {
                new PharmacistResponse { Id = 1, SpecializationId = 1 },
                new PharmacistResponse { Id = 2, SpecializationId = 1 }
            };

            _pharmacistServiceMock.Setup(service => service.GetAllPharmacistsAsync()).ReturnsAsync(pharmacists);

            // Act
            var result = await _controller.GetAllPharmacists();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<PharmacistResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Pharmacists fetched successfully", apiResponse.Message);
            Assert.Equal(pharmacists, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllPharmacists_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _pharmacistServiceMock.Setup(service => service.GetAllPharmacistsAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllPharmacists();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetPharmacistById_WhenPharmacistExists_ShouldReturnOk()
        {
            // Arrange
            var pharmacistId = 1;
            var pharmacist = new PharmacistResponse { Id = pharmacistId, SpecializationId = 1 };

            _pharmacistServiceMock.Setup(service => service.GetPharmacistByIdAsync(pharmacistId)).ReturnsAsync(pharmacist);

            // Act
            var result = await _controller.GetPharmacistById(pharmacistId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Pharmacist fetched successfully", apiResponse.Message);
            Assert.Equal(pharmacist, apiResponse.Data);
        }

        [Fact]
        public async Task GetPharmacistById_WhenPharmacistDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var pharmacistId = 1;
            _pharmacistServiceMock.Setup(service => service.GetPharmacistByIdAsync(pharmacistId)).ThrowsAsync(new NotFoundException("Pharmacist not found"));

            // Act
            var result = await _controller.GetPharmacistById(pharmacistId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Pharmacist not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenPharmacistIsValid_ShouldReturnOk()
        {
            // Arrange
            var pharmacistRequest = new PharmacistRequest { SpecializationId = 1 };
            var addedPharmacist = new PharmacistResponse { Id = 1, SpecializationId = 1 };

            _pharmacistServiceMock.Setup(service => service.CreatePharmacistAsync(pharmacistRequest)).ReturnsAsync(addedPharmacist);

            // Act
            var result = await _controller.Add(pharmacistRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Pharmacist added successfully", apiResponse.Message);
            Assert.Equal(addedPharmacist, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var pharmacistRequest = new PharmacistRequest { SpecializationId = 1 };
            _pharmacistServiceMock.Setup(service => service.CreatePharmacistAsync(pharmacistRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(pharmacistRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenPharmacistExists_ShouldReturnOk()
        {
            // Arrange
            var pharmacistId = 1;
            var pharmacistRequest = new PharmacistRequest { SpecializationId = 1 };
            var updatedPharmacist = new PharmacistResponse { Id = pharmacistId, SpecializationId = 1 };

            _pharmacistServiceMock.Setup(service => service.UpdatePharmacistAsync(pharmacistId, pharmacistRequest)).ReturnsAsync(updatedPharmacist);

            // Act
            var result = await _controller.Update(pharmacistId, pharmacistRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Pharmacist updated successfully", apiResponse.Message);
            Assert.Equal(updatedPharmacist, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenPharmacistDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var pharmacistId = 1;
            var pharmacistRequest = new PharmacistRequest { SpecializationId = 1 };
            _pharmacistServiceMock.Setup(service => service.UpdatePharmacistAsync(pharmacistId, pharmacistRequest)).ThrowsAsync(new NotFoundException("Pharmacist not found"));

            // Act
            var result = await _controller.Update(pharmacistId, pharmacistRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Pharmacist not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var pharmacistId = 1;
            var pharmacistRequest = new PharmacistRequest { SpecializationId =1 };
            _pharmacistServiceMock.Setup(service => service.UpdatePharmacistAsync(pharmacistId, pharmacistRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(pharmacistId, pharmacistRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenPharmacistExists_ShouldReturnOk()
        {
            // Arrange
            var pharmacistId = 1;
            var deletedPharmacist = new Pharmacist { Id = pharmacistId, SpecializationId = 1 };

            _pharmacistServiceMock.Setup(service => service.DeletePharmacistAsync(pharmacistId)).ReturnsAsync(deletedPharmacist);

            // Act
            var result = await _controller.Delete(pharmacistId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Pharmacist deleted successfully", apiResponse.Message);
            Assert.Equal(deletedPharmacist, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenPharmacistDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var pharmacistId = 1;
            _pharmacistServiceMock.Setup(service => service.DeletePharmacistAsync(pharmacistId)).ThrowsAsync(new NotFoundException("Pharmacist not found"));

            // Act
            var result = await _controller.Delete(pharmacistId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Pharmacist not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var pharmacistId = 1;
            _pharmacistServiceMock.Setup(service => service.DeletePharmacistAsync(pharmacistId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(pharmacistId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
