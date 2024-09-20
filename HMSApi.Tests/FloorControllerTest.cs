using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccessLayer.Entities;
using HMS.Controllers;
using LMSApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SharedClasses.Exceptions;
using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMSApi.Tests
{
    public class FloorControllerTest
    {
        private readonly Mock<IFloorService> _mockFloorService;
        private readonly Mock<ILogger<FloorsController>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly FloorsController _controller;


        public FloorControllerTest()
        {
            _mockFloorService = new Mock<IFloorService>();
            _mockLogger = new Mock<ILogger<FloorsController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new FloorsController(_mockFloorService.Object, _mockLogger.Object, _mapper);
        }

        [Fact]
        public async Task GetAllFloors_WhenFloorsExist_ShouldReturnOk()
        {
            // Arrange
            var floors = new List<Floor>
            {
                new Floor { Id = 1, Name = "First Floor" },
                new Floor { Id = 2, Name = "Second Floor" }
            };
            var floorResponses = _mapper.Map<List<FloorResponse>>(floors);

            _mockFloorService.Setup(f => f.GetAllFloorsAsync())
                .ReturnsAsync(floorResponses);

            // Act
            var result = await _controller.GetAllFloors();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<FloorResponse>>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(floorResponses, apiResponse.Data);
        }
        [Fact]
        public async Task GetAllFloors_OnExceptions_ReturnsInternalServerError()
        {
            // Arrange
            _mockFloorService.Setup(service => service.GetAllFloorsAsync()).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _controller.GetAllFloors();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Error", apiResponse.Message);
        }

        [Fact]
        public async Task GetFloorById_WithFloor_ReturnsOkResult()
        {
            // Arrange
            var floor = new Floor { Id = 1, Name = "First Floor" };
            var floorResponse = new FloorResponse { Id = 1, Name = "First Floor" };

            _mockFloorService.Setup(service => service.GetFloorByIdAsync(1)).ReturnsAsync(floorResponse);
           

            // Act
            var result = await _controller.GetFloorById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Floor fetched successfully", apiResponse.Message);
            Assert.Equal(floorResponse, apiResponse.Data);
        }

        [Fact]
        public async Task GetFloorById_WhenFloorDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _mockFloorService.Setup(service => service.GetFloorByIdAsync(1)).ThrowsAsync(new NotFoundException("Floor not found"));

            // Act
            var result = await _controller.GetFloorById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Floor not found", apiResponse.Message);
        }

        [Fact]
        public async Task GetFloorById_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _mockFloorService.Setup(service => service.GetFloorByIdAsync(1)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetFloorById(1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenFloorIsAdded_ShouldReturnOk()
        {
            // Arrange
            var floorRequest = new FloorRequest { Name = "New Floor" };
            var addedFloor = new Floor { Id = 1, Name = "New Floor" };
            var floorResponse = new FloorResponse { Id = 1, Name = "New Floor" };

            _mockFloorService.Setup(service => service.CreateFloorAsync(floorRequest)).ReturnsAsync(floorResponse);
          

            // Act
            var result = await _controller.Add(floorRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Floor added successfully", apiResponse.Message);
            Assert.Equal(floorResponse, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var floorRequest = new FloorRequest { Name = "New Floor" };
            _mockFloorService.Setup(service => service.CreateFloorAsync(floorRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(floorRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenFloorIsUpdated_ShouldReturnOk()
        {
            // Arrange
            var floorRequest = new FloorRequest { Name = "Updated Floor" };
            var updatedFloor = new Floor { Id = 1, Name = "Updated Floor" };
            var floorResponse = new FloorResponse { Id = 1, Name = "Updated Floor" };

            _mockFloorService.Setup(service => service.UpdateFloorAsync(1, floorRequest)).ReturnsAsync(floorResponse);

            // Act
            var result = await _controller.Update(1, floorRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Floor updated successfully", apiResponse.Message);
            Assert.Equal(floorResponse, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenFloorDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var floorRequest = new FloorRequest { Name = "Updated Floor" };
            _mockFloorService.Setup(service => service.UpdateFloorAsync(1, floorRequest)).ThrowsAsync(new NotFoundException("Floor not found"));

            // Act
            var result = await _controller.Update(1, floorRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Floor not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var floorRequest = new FloorRequest { Name = "Updated Floor" };
            _mockFloorService.Setup(service => service.UpdateFloorAsync(1, floorRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(1, floorRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
        [Fact]
        public async Task Delete_WhenFloorIsDeleted_ShouldReturnOk()
        {
            // Arrange
            var deletedFloor = new Floor { Id = 1, Name = "Deleted Floor" };

            _mockFloorService.Setup(service => service.DeleteFloorAsync(1)).ReturnsAsync(deletedFloor);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Floor deleted successfully", apiResponse.Message);
            Assert.Equal(deletedFloor, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenFloorDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _mockFloorService.Setup(service => service.DeleteFloorAsync(1)).ThrowsAsync(new NotFoundException("Floor not found"));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Floor not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _mockFloorService.Setup(service => service.DeleteFloorAsync(1)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}

