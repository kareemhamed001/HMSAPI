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
    public class SpecializationsControllerTest
    {
        private readonly Mock<ISpecializationService> _specializationServiceMock;
        private readonly Mock<ILogger<SpecializationsController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly SpecializationsController _controller;

        public SpecializationsControllerTest()
        {
            _specializationServiceMock = new Mock<ISpecializationService>();
            _loggerMock = new Mock<ILogger<SpecializationsController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new SpecializationsController(_specializationServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllSpecializations_WhenSpecializationsExist_ShouldReturnOk()
        {
            // Arrange
            var specializations = new List<SpecializationResponse>
            {
                new SpecializationResponse { Id = 1, Name = "Specialization A" },
                new SpecializationResponse { Id = 2, Name = "Specialization B" }
            };

            _specializationServiceMock.Setup(service => service.GetAllSpecializationsAsync()).ReturnsAsync(specializations);

            // Act
            var result = await _controller.GetAllSpecializations();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<SpecializationResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Specializations fetched successfully", apiResponse.Message);
            Assert.Equal(specializations, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllSpecializations_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _specializationServiceMock.Setup(service => service.GetAllSpecializationsAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllSpecializations();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetSpecializationById_WhenSpecializationExists_ShouldReturnOk()
        {
            // Arrange
            var specializationId = 1;
            var specialization = new SpecializationResponse { Id = specializationId, Name = "Specialization A" };

            _specializationServiceMock.Setup(service => service.GetSpecializationByIdAsync(specializationId)).ReturnsAsync(specialization);

            // Act
            var result = await _controller.GetSpecializationById(specializationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Specialization fetched successfully", apiResponse.Message);
            Assert.Equal(specialization, apiResponse.Data);
        }

        [Fact]
        public async Task GetSpecializationById_WhenSpecializationDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var specializationId = 1;
            _specializationServiceMock.Setup(service => service.GetSpecializationByIdAsync(specializationId)).ThrowsAsync(new NotFoundException("Specialization not found"));

            // Act
            var result = await _controller.GetSpecializationById(specializationId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Specialization not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenSpecializationIsValid_ShouldReturnOk()
        {
            // Arrange
            var specializationRequest = new SpecializationRequest { Name = "Specialization A" };
            var addedSpecialization = new SpecializationResponse { Id = 1, Name = "Specialization A" };

            _specializationServiceMock.Setup(service => service.CreateSpecializationAsync(specializationRequest)).ReturnsAsync(addedSpecialization);

            // Act
            var result = await _controller.Add(specializationRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Specialization added successfully", apiResponse.Message);
            Assert.Equal(addedSpecialization, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var specializationRequest = new SpecializationRequest { Name = "Specialization A" };
            _specializationServiceMock.Setup(service => service.CreateSpecializationAsync(specializationRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(specializationRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenSpecializationExists_ShouldReturnOk()
        {
            // Arrange
            var specializationId = 1;
            var specializationRequest = new SpecializationRequest { Name = "Updated Specialization" };
            var updatedSpecialization = new SpecializationResponse { Id = specializationId, Name = "Updated Specialization" };

            _specializationServiceMock.Setup(service => service.UpdateSpecializationAsync(specializationId, specializationRequest)).ReturnsAsync(updatedSpecialization);

            // Act
            var result = await _controller.Update(specializationId, specializationRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Specialization updated successfully", apiResponse.Message);
            Assert.Equal(updatedSpecialization, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenSpecializationDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var specializationId = 1;
            var specializationRequest = new SpecializationRequest { Name = "Updated Specialization" };
            _specializationServiceMock.Setup(service => service.UpdateSpecializationAsync(specializationId, specializationRequest)).ThrowsAsync(new NotFoundException("Specialization not found"));

            // Act
            var result = await _controller.Update(specializationId, specializationRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Specialization not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var specializationId = 1;
            var specializationRequest = new SpecializationRequest { Name = "Updated Specialization" };
            _specializationServiceMock.Setup(service => service.UpdateSpecializationAsync(specializationId, specializationRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(specializationId, specializationRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenSpecializationExists_ShouldReturnOk()
        {
            // Arrange
            var specializationId = 1;
            var deletedSpecialization = new Specialization { Id = specializationId, Name = "Specialization A" };

            _specializationServiceMock.Setup(service => service.DeleteSpecializationAsync(specializationId)).ReturnsAsync(deletedSpecialization);

            // Act
            var result = await _controller.Delete(specializationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Specialization deleted successfully", apiResponse.Message);
            Assert.Equal(deletedSpecialization, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenSpecializationDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var specializationId = 1;
            _specializationServiceMock.Setup(service => service.DeleteSpecializationAsync(specializationId)).ThrowsAsync(new NotFoundException("Specialization not found"));

            // Act
            var result = await _controller.Delete(specializationId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Specialization not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var specializationId = 1;
            _specializationServiceMock.Setup(service => service.DeleteSpecializationAsync(specializationId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(specializationId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
