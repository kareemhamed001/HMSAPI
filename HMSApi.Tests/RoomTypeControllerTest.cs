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
    public class RoomTypeControllerTest
    {
        private readonly Mock<IRoomTypeService> _mockRoomTypeService;
        private readonly Mock<ILogger<RoomTypesController>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly RoomTypesController _controller;

        public RoomTypeControllerTest()
        {
            _mockRoomTypeService = new Mock<IRoomTypeService>();
            _mockLogger = new Mock<ILogger<RoomTypesController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new RoomTypesController(_mockRoomTypeService.Object, _mockLogger.Object, _mapper);
        }
        [Fact]
        public async Task GetAllRoomTypes_WhenRoomTypesExist_ShouldReturnOk()
        {
            // Arrange
            var roomTypes = new List<RoomTypeResponse>
        {
            new RoomTypeResponse { Id = 1, Name = "Deluxe" },
            new RoomTypeResponse { Id = 2, Name = "Standard" }
        };

            _mockRoomTypeService.Setup(service => service.GetAllRoomTypesAsync()).ReturnsAsync(roomTypes);

            // Act
            var result = await _controller.GetAllRoomTypes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<RoomTypeResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Room types fetched successfully", apiResponse.Message);
            Assert.Equal(roomTypes, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllRoomTypes_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _mockRoomTypeService.Setup(service => service.GetAllRoomTypesAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllRoomTypes();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetRoomTypeById_WhenRoomTypeExists_ShouldReturnOk()
        {
            // Arrange
            var roomTypeId = 1;
            var roomType = new RoomTypeResponse { Id = roomTypeId, Name = "Deluxe" };

            _mockRoomTypeService.Setup(service => service.GetRoomTypeByIdAsync(roomTypeId)).ReturnsAsync(roomType);

            // Act
            var result = await _controller.GetRoomTypeById(roomTypeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Room type fetched successfully", apiResponse.Message);
            Assert.Equal(roomType, apiResponse.Data);
        }

        [Fact]
        public async Task GetRoomTypeById_WhenRoomTypeDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var roomTypeId = 1;
            _mockRoomTypeService.Setup(service => service.GetRoomTypeByIdAsync(roomTypeId)).ThrowsAsync(new NotFoundException("Room type not found"));

            // Act
            var result = await _controller.GetRoomTypeById(roomTypeId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Room type not found", apiResponse.Message);
        }

        [Fact]
        public async Task GetRoomTypeById_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var roomTypeId = 1;
            _mockRoomTypeService.Setup(service => service.GetRoomTypeByIdAsync(roomTypeId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetRoomTypeById(roomTypeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenRoomTypeIsAdded_ShouldReturnOk()
        {
            // Arrange
            var roomTypeRequest = new RoomTypeRequest { Name = "Deluxe" };
            var addedRoomType = new RoomTypeResponse { Id = 1, Name = "Deluxe" };

            _mockRoomTypeService.Setup(service => service.CreateRoomTypeAsync(roomTypeRequest)).ReturnsAsync(addedRoomType);

            // Act
            var result = await _controller.Add(roomTypeRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Room type added successfully", apiResponse.Message);
            Assert.Equal(addedRoomType, apiResponse.Data);
        }
        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var roomTypeRequest = new RoomTypeRequest { Name = "Deluxe" };
            _mockRoomTypeService.Setup(service => service.CreateRoomTypeAsync(roomTypeRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(roomTypeRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenRoomTypeExists_ShouldReturnOk()
        {
            // Arrange
            var roomTypeId = 1;
            var roomTypeRequest = new RoomTypeRequest { Name = "Deluxe Updated" };
            var updatedRoomType = new RoomTypeResponse { Id = roomTypeId, Name = "Deluxe Updated" };

            _mockRoomTypeService.Setup(service => service.UpdateRoomTypeAsync(roomTypeId, roomTypeRequest)).ReturnsAsync(updatedRoomType);

            // Act
            var result = await _controller.Update(roomTypeId, roomTypeRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Room type updated successfully", apiResponse.Message);
            Assert.Equal(updatedRoomType, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenRoomTypeDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var roomTypeId = 1;
            var roomTypeRequest = new RoomTypeRequest { Name = "Deluxe Updated" };
            _mockRoomTypeService.Setup(service => service.UpdateRoomTypeAsync(roomTypeId, roomTypeRequest)).ThrowsAsync(new NotFoundException("Room type not found"));

            // Act
            var result = await _controller.Update(roomTypeId, roomTypeRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Room type not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var roomTypeId = 1;
            var roomTypeRequest = new RoomTypeRequest { Name = "Deluxe Updated" };
            _mockRoomTypeService.Setup(service => service.UpdateRoomTypeAsync(roomTypeId, roomTypeRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(roomTypeId, roomTypeRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
        [Fact]
        public async Task Delete_WhenRoomTypeExists_ShouldReturnOk()
        {
            // Arrange
            var roomTypeId = 1;
            var deletedRoomType = new RoomType { Id = roomTypeId, Name = "Deluxe" };

            _mockRoomTypeService.Setup(service => service.DeleteRoomTypeAsync(roomTypeId)).ReturnsAsync(deletedRoomType);

            // Act
            var result = await _controller.Delete(roomTypeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Room type deleted successfully", apiResponse.Message);
            Assert.Equal(deletedRoomType, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenRoomTypeDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var roomTypeId = 1;
            _mockRoomTypeService.Setup(service => service.DeleteRoomTypeAsync(roomTypeId)).ThrowsAsync(new NotFoundException("Room type not found"));

            // Act
            var result = await _controller.Delete(roomTypeId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Room type not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var roomTypeId = 1;
            _mockRoomTypeService.Setup(service => service.DeleteRoomTypeAsync(roomTypeId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(roomTypeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
