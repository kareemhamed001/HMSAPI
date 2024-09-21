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
    public class StaffControllerTest
    {
        private readonly Mock<IStaffService> _staffServiceMock;
        private readonly Mock<ILogger<StaffController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly StaffController _controller;

        public StaffControllerTest()
        {
            _staffServiceMock = new Mock<IStaffService>();
            _loggerMock = new Mock<ILogger<StaffController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new StaffController(_staffServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllStaff_WhenStaffMembersExist_ShouldReturnOk()
        {
            // Arrange
            var staffMembers = new List<StaffResponse>
            {
                new StaffResponse { Id = 1, Position = "Staff A" },
                new StaffResponse { Id = 2, Position = "Staff B" }
            };

            _staffServiceMock.Setup(service => service.GetAllStaffAsync()).ReturnsAsync(staffMembers);

            // Act
            var result = await _controller.GetAllStaff();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<StaffResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Staff fetched successfully", apiResponse.Message);
            Assert.Equal(staffMembers, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllStaff_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _staffServiceMock.Setup(service => service.GetAllStaffAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllStaff();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetStaffById_WhenStaffExists_ShouldReturnOk()
        {
            // Arrange
            var staffId = 1;
            var staff = new StaffResponse { Id = staffId, Position = "Staff A" };

            _staffServiceMock.Setup(service => service.GetStaffByIdAsync(staffId)).ReturnsAsync(staff);

            // Act
            var result = await _controller.GetStaffById(staffId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Staff fetched successfully", apiResponse.Message);
            Assert.Equal(staff, apiResponse.Data);
        }

        [Fact]
        public async Task GetStaffById_WhenStaffDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var staffId = 1;
            _staffServiceMock.Setup(service => service.GetStaffByIdAsync(staffId)).ThrowsAsync(new NotFoundException("Staff not found"));

            // Act
            var result = await _controller.GetStaffById(staffId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Staff not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenStaffIsValid_ShouldReturnOk()
        {
            // Arrange
            var staffRequest = new StaffRequest { Position = "Staff A" };
            var addedStaff = new StaffResponse { Id = 1, Position = "Staff A" };

            _staffServiceMock.Setup(service => service.CreateStaffAsync(staffRequest)).ReturnsAsync(addedStaff);

            // Act
            var result = await _controller.Add(staffRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Staff added successfully", apiResponse.Message);
            Assert.Equal(addedStaff, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var staffRequest = new StaffRequest { Position = "Staff A" };
            _staffServiceMock.Setup(service => service.CreateStaffAsync(staffRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(staffRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenStaffExists_ShouldReturnOk()
        {
            // Arrange
            var staffId = 1;
            var staffRequest = new StaffRequest { Position = "Updated Staff" };
            var updatedStaff = new StaffResponse { Id = staffId, Position = "Updated Staff" };

            _staffServiceMock.Setup(service => service.UpdateStaffAsync(staffId, staffRequest)).ReturnsAsync(updatedStaff);

            // Act
            var result = await _controller.Update(staffId, staffRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Staff updated successfully", apiResponse.Message);
            Assert.Equal(updatedStaff, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenStaffDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var staffId = 1;
            var staffRequest = new StaffRequest { Position = "Updated Staff" };
            _staffServiceMock.Setup(service => service.UpdateStaffAsync(staffId, staffRequest)).ThrowsAsync(new NotFoundException("Staff not found"));

            // Act
            var result = await _controller.Update(staffId, staffRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Staff not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var staffId = 1;
            var staffRequest = new StaffRequest { Position = "Updated Staff" };
            _staffServiceMock.Setup(service => service.UpdateStaffAsync(staffId, staffRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(staffId, staffRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenStaffExists_ShouldReturnOk()
        {
            // Arrange
            var staffId = 1;
            var deletedStaff = new Staff { Id = staffId, Position = "Staff A" };

            _staffServiceMock.Setup(service => service.DeleteStaffAsync(staffId)).ReturnsAsync(deletedStaff);

            // Act
            var result = await _controller.Delete(staffId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Staff deleted successfully", apiResponse.Message);
            Assert.Equal(deletedStaff, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenStaffDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var staffId = 1;
            _staffServiceMock.Setup(service => service.DeleteStaffAsync(staffId)).ThrowsAsync(new NotFoundException("Staff not found"));

            // Act
            var result = await _controller.Delete(staffId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Staff not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var staffId = 1;
            _staffServiceMock.Setup(service => service.DeleteStaffAsync(staffId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(staffId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
