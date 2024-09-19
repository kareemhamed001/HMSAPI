
using AutoMapper;
using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
using DataAccessLayer.Entities;

using LMSApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SharedClasses.Exceptions;
using SharedClasses.Responses;
using System.Security.Claims;


namespace HMSApi.Tests
{
    public class BuildingsControllerTests
    {
        private readonly Mock<IBuildingService> _mockBuildingService;
        private readonly Mock<ILogger<BuldingsController>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly BuldingsController _controller;

        public BuildingsControllerTests()
        {
            _mockBuildingService = new Mock<IBuildingService>();
            _mockLogger = new Mock<ILogger<BuldingsController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new BuldingsController(_mockBuildingService.Object, _mockLogger.Object, _mapper);
        }
        [Fact]
        public async Task GetAllBuildings_WhenBuildingsExist_ShouldReturnOk()
        {
            // Arrange
            List<Building> Buildinges = new List<Building>
            {
                new Building { Id = 1, Name = "class 1", Description = "class1" },
                new Building { Id = 2, Name = "class 2", Description = "class2" }
            };

            List<BuildingResponse> BuildingesResponse = new List<BuildingResponse>
            {
                new BuildingResponse { Id = 1, Name = "class 1", Description = "class1" },
                new BuildingResponse { Id = 2, Name = "class 2", Description = "class2" }
            };

            _mockBuildingService.Setup(b => b.GetAllBuildingsAsync())
                .ReturnsAsync(BuildingesResponse);

            // Act
            var result = await _controller.GetAllBuildings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<BuildingResponse>>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(BuildingesResponse, apiResponse.Data);

        }
        [Fact]
        public async Task GetAllBuildings_WhenNoBuildingsExist_ShouldReturnEmptyList()
        {
            // Arrange
            List<BuildingResponse> emptyResponse = new List<BuildingResponse>();

            _mockBuildingService.Setup(b => b.GetAllBuildingsAsync())
                .ReturnsAsync(emptyResponse);

            // Act
            var result = await _controller.GetAllBuildings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<BuildingResponse>>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Empty(apiResponse.Data);
        }

        [Fact]
        public async Task GetAllBuildings_WhenServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            _mockBuildingService.Setup(b => b.GetAllBuildingsAsync())
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.GetAllBuildings();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);

            // Update here to reflect the correct response type
            var apiResponse = Assert.IsType<ApiResponseBase>(statusResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Service error", apiResponse.Message);
        }

        [Fact]
        public async Task GetBuildingById_WhenBuildingExists_ShouldReturnOk()
        {
            // Arrange
            var building = new Building { Id = 1, Name = "class 1", Description = "class1" };
            var buildingResponse = new BuildingResponse { Id = 1, Name = "class 1", Description = "class1" };

            _mockBuildingService.Setup(b => b.GetBuildingByIdAsync(1))
                .ReturnsAsync(buildingResponse);

            // Act
            var result = await _controller.GetBuildingById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(buildingResponse, apiResponse.Data);
        }
        [Fact]
        public async Task GetBuildingById_WhenBuildingDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _mockBuildingService.Setup(b => b.GetBuildingByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new NotFoundException("Building not found"));

            // Act
            var result = await _controller.GetBuildingById(999); // Non-existing ID

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("Building not found", apiResponse.Message);
        }

        [Fact]
        public async Task GetBuildingById_WhenServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            _mockBuildingService.Setup(b => b.GetBuildingByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.GetBuildingById(1); // Valid ID

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(statusResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Service error", apiResponse.Message);
        }
        [Fact]
        public async Task Add_WhenBuildingIsCreated_ShouldReturnOk()
        {
            // Arrange
            var buildingRequest = new BuildingRequest { Name = "class 1", Description = "class1" };
            var addedBuilding = new Building { Id = 1, Name = "class 1", Description = "class1" };
            var buildingResponse = new BuildingResponse { Id = 1, Name = "class 1", Description = "class1" };

            _mockBuildingService.Setup(b => b.CreateBuildingAsync(buildingRequest))
                .ReturnsAsync(buildingResponse);

            // Act
            var result = await _controller.Add(buildingRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(buildingResponse, apiResponse.Data);
        }
        [Fact]
        public async Task Add_WhenServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var buildingRequest = new BuildingRequest { Name = "class 1", Description = "class1" };

            _mockBuildingService.Setup(b => b.CreateBuildingAsync(buildingRequest))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.Add(buildingRequest);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(statusResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Service error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenBuildingExists_ShouldReturnOk()
        {
            // Arrange
            var buildingRequest = new BuildingRequest { Name = "Updated class 1", Description = "Updated description" };
            var updatedBuilding = new Building { Id = 1, Name = "Updated class 1", Description = "Updated description" };
            var buildingResponse = new BuildingResponse { Id = 1, Name = "Updated class 1", Description = "Updated description" };

            _mockBuildingService.Setup(b => b.UpdateBuildingAsync(1, buildingRequest))
                .ReturnsAsync(buildingResponse);

            // Act
            var result = await _controller.Update(1, buildingRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(buildingResponse, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenBuildingDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var buildingRequest = new BuildingRequest { Name = "Non-existing building", Description = "Description" };

            _mockBuildingService.Setup(b => b.UpdateBuildingAsync(It.IsAny<int>(), buildingRequest))
                .ThrowsAsync(new NotFoundException("Building not found"));

            // Act
            var result = await _controller.Update(999, buildingRequest); // Non-existing ID

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("Building not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var buildingRequest = new BuildingRequest { Name = "Valid building", Description = "Description" };

            _mockBuildingService.Setup(b => b.UpdateBuildingAsync(It.IsAny<int>(), buildingRequest))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.Update(1, buildingRequest); // Valid ID

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(statusResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Service error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenBuildingExists_ShouldReturnOk()
        {
            // Arrange
            int buildingId = 1;
            var deletedBuilding = new Building { Id = 1, Name = "Updated class 1", Description = "Updated description" };
            _mockBuildingService.Setup(b => b.DeleteBuildingAsync(buildingId))
                .ReturnsAsync(deletedBuilding);

            // Act
            var result = await _controller.Delete(buildingId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(deletedBuilding, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenBuildingDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            int buildingId = 999; // Non-existing ID

            _mockBuildingService.Setup(b => b.DeleteBuildingAsync(buildingId))
                .ThrowsAsync(new NotFoundException("Building not found"));

            // Act
            var result = await _controller.Delete(buildingId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("Building not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            int buildingId = 1;

            _mockBuildingService.Setup(b => b.DeleteBuildingAsync(buildingId))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.Delete(buildingId);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(statusResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Service error", apiResponse.Message);
        }
        [Fact]
        public async Task GetFloorsByBuildingId_WhenFloorsExist_ShouldReturnOk()
        {
            // Arrange
            int buildingId = 1;
            var floors = new List<Floor>
              {
                 new Floor { Id = 1 ,Name = "First Floor" },
                 new Floor { Id = 1,  Name = "secound Floor" },
              };

            _mockBuildingService.Setup(b => b.GetFloorsByBuildingIdAsync(buildingId))
                .ReturnsAsync(floors);

            // Act
            var result = await _controller.GetFloorsByBuildingId(buildingId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<Floor>>(okResult.Value);
            Assert.Equal(200, apiResponse.Status);
            Assert.True(apiResponse.Success);
            Assert.Equal(floors, apiResponse.Data);
        }

        [Fact]
        public async Task GetFloorsByBuildingId_WhenBuildingDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            int buildingId = 999; // Non-existing ID

            _mockBuildingService.Setup(b => b.GetFloorsByBuildingIdAsync(buildingId))
                .ThrowsAsync(new NotFoundException("Building not found"));

            // Act
            var result = await _controller.GetFloorsByBuildingId(buildingId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.Equal(404, apiResponse.Status);
            Assert.False(apiResponse.Success);
            Assert.Equal("Building not found", apiResponse.Message);
        }

        [Fact]
        public async Task GetFloorsByBuildingId_WhenServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            int buildingId = 1;

            _mockBuildingService.Setup(b => b.GetFloorsByBuildingIdAsync(buildingId))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.GetFloorsByBuildingId(buildingId);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponseBase>(statusResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Service error", apiResponse.Message);
        }
    }

}