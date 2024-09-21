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
    public class NursesControllerTest
    {
        private readonly Mock<INurseService> _nurseServiceMock;
        private readonly Mock<ILogger<NursesController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly NursesController _controller;

        public NursesControllerTest()
        {
            _nurseServiceMock = new Mock<INurseService>();
            _loggerMock = new Mock<ILogger<NursesController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new NursesController(_nurseServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllNurses_WhenNursesExist_ShouldReturnOk()
        {
            // Arrange
            var nurses = new List<NurseResponse>
            {
                new NurseResponse { Id = 1, Passport = "Nurse A" },
                new NurseResponse { Id = 2, Passport = "Nurse B" }
            };

            _nurseServiceMock.Setup(service => service.GetAllNursesAsync()).ReturnsAsync(nurses);

            // Act
            var result = await _controller.GetAllNurses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<NurseResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Nurses fetched successfully", apiResponse.Message);
            Assert.Equal(nurses, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllNurses_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _nurseServiceMock.Setup(service => service.GetAllNursesAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllNurses();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetNurseById_WhenNurseExists_ShouldReturnOk()
        {
            // Arrange
            var nurseId = 1;
            var nurse = new NurseResponse { Id = nurseId, Passport = "Nurse A" };

            _nurseServiceMock.Setup(service => service.GetNurseByIdAsync(nurseId)).ReturnsAsync(nurse);

            // Act
            var result = await _controller.GetNurseById(nurseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Nurse fetched successfully", apiResponse.Message);
            Assert.Equal(nurse, apiResponse.Data);
        }

        [Fact]
        public async Task GetNurseById_WhenNurseDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var nurseId = 1;
            _nurseServiceMock.Setup(service => service.GetNurseByIdAsync(nurseId)).ThrowsAsync(new NotFoundException("Nurse not found"));

            // Act
            var result = await _controller.GetNurseById(nurseId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Nurse not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenNurseIsValid_ShouldReturnOk()
        {
            // Arrange
            var nurseRequest = new NurseRequest { Passport = "Nurse A" };
            var addedNurse = new NurseResponse { Id = 1, Passport = "Nurse A" };

            _nurseServiceMock.Setup(service => service.CreateNurseAsync(nurseRequest)).ReturnsAsync(addedNurse);

            // Act
            var result = await _controller.Add(nurseRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Nurse added successfully", apiResponse.Message);
            Assert.Equal(addedNurse, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var nurseRequest = new NurseRequest { Passport = "Nurse A" };
            _nurseServiceMock.Setup(service => service.CreateNurseAsync(nurseRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(nurseRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenNurseExists_ShouldReturnOk()
        {
            // Arrange
            var nurseId = 1;
            var nurseRequest = new NurseRequest { Passport = "Updated Nurse" };
            var updatedNurse = new NurseResponse { Id = nurseId, Passport = "Updated Nurse" };

            _nurseServiceMock.Setup(service => service.UpdateNurseAsync(nurseId, nurseRequest)).ReturnsAsync(updatedNurse);

            // Act
            var result = await _controller.Update(nurseId, nurseRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Nurse updated successfully", apiResponse.Message);
            Assert.Equal(updatedNurse, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenNurseDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var nurseId = 1;
            var nurseRequest = new NurseRequest { Passport = "Updated Nurse" };
            _nurseServiceMock.Setup(service => service.UpdateNurseAsync(nurseId, nurseRequest)).ThrowsAsync(new NotFoundException("Nurse not found"));

            // Act
            var result = await _controller.Update(nurseId, nurseRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Nurse not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var nurseId = 1;
            var nurseRequest = new NurseRequest { Passport = "Updated Nurse" };
            _nurseServiceMock.Setup(service => service.UpdateNurseAsync(nurseId, nurseRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(nurseId, nurseRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenNurseExists_ShouldReturnOk()
        {
            // Arrange
            var nurseId = 1;
            var deletedNurse = new Nurse { Id = nurseId, Passport = "Nurse A" };

            _nurseServiceMock.Setup(service => service.DeleteNurseAsync(nurseId)).ReturnsAsync(deletedNurse);

            // Act
            var result = await _controller.Delete(nurseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Nurse deleted successfully", apiResponse.Message);
            Assert.Equal(deletedNurse, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenNurseDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var nurseId = 1;
            _nurseServiceMock.Setup(service => service.DeleteNurseAsync(nurseId)).ThrowsAsync(new NotFoundException("Nurse not found"));

            // Act
            var result = await _controller.Delete(nurseId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Nurse not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var nurseId = 1;
            _nurseServiceMock.Setup(service => service.DeleteNurseAsync(nurseId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(nurseId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
