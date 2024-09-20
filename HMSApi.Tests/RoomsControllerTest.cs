using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
using Xunit;

public class RoomControllerTests
{
    private readonly Mock<IRoomService> _roomServiceMock;
    private readonly Mock<ILogger<RoomsController>> _loggerMock;
    private readonly IMapper _mapper;
    private readonly RoomsController _controller;

    public RoomControllerTests()
    {
        _roomServiceMock = new Mock<IRoomService>();
        _loggerMock = new Mock<ILogger<RoomsController>>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
        _mapper = config.CreateMapper();
        _controller = new RoomsController(_roomServiceMock.Object,_loggerMock.Object, _mapper);
    }
    [Fact]
    public async Task GetAllRooms_WhenRoomsExist_ShouldReturnOk()
    {
        // Arrange
        var rooms = new List<RoomResponse>
        {
            new RoomResponse { Id = 1, Name = "Room A" },
            new RoomResponse { Id = 2, Name = "Room B" }
        };

        _roomServiceMock.Setup(service => service.GetAllRoomAsync()).ReturnsAsync(rooms);

        // Act
        var result = await _controller.GetAllRooms();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseListStrategy<RoomResponse>>(okResult.Value);
        Assert.True(apiResponse.Success);
        Assert.Equal(200, apiResponse.Status);
        Assert.Equal("Rooms fetched successfully", apiResponse.Message);
        Assert.Equal(rooms, apiResponse.Data);
    }

    [Fact]
    public async Task GetAllRooms_WhenExceptionOccurs_ShouldReturnInternalServerError()
    {
        // Arrange
        _roomServiceMock.Setup(service => service.GetAllRoomAsync()).ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.GetAllRooms();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
        Assert.False(apiResponse.Success);
        Assert.Equal(500, apiResponse.Status);
        Assert.Equal("Unexpected error", apiResponse.Message);
    }

    [Fact]
    public async Task GetRoomById_WhenRoomExists_ShouldReturnOk()
    {
        // Arrange
        var roomId = 1;
        var room = new RoomResponse { Id = roomId, Name = "Room A" };

        _roomServiceMock.Setup(service => service.GetRoomByIdAsync(roomId)).ReturnsAsync(room);

        // Act
        var result = await _controller.GetRoomById(roomId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
        Assert.True(apiResponse.Success);
        Assert.Equal(200, apiResponse.Status);
        Assert.Equal("Room fetched successfully", apiResponse.Message);
        Assert.Equal(room, apiResponse.Data);
    }

    [Fact]
    public async Task GetRoomById_WhenRoomDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var roomId = 1;
        _roomServiceMock.Setup(service => service.GetRoomByIdAsync(roomId)).ThrowsAsync(new NotFoundException("Room not found"));

        // Act
        var result = await _controller.GetRoomById(roomId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
        Assert.False(apiResponse.Success);
        Assert.Equal(404, apiResponse.Status);
        Assert.Equal("Room not found", apiResponse.Message);
    }

    [Fact]
    public async Task GetRoomById_WhenExceptionOccurs_ShouldReturnInternalServerError()
    {
        // Arrange
        var roomId = 1;
        _roomServiceMock.Setup(service => service.GetRoomByIdAsync(roomId)).ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.GetRoomById(roomId);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
        Assert.False(apiResponse.Success);
        Assert.Equal(500, apiResponse.Status);
        Assert.Equal("Unexpected error", apiResponse.Message);
    }
    [Fact]
    public async Task Add_WhenRoomIsValid_ShouldReturnOk()
    {
        // Arrange
        var roomRequest = new RoomRequest { Name = "Room A" };
        var addedRoom = new RoomResponse { Id = 1, Name = "Room A" };

        _roomServiceMock.Setup(service => service.CreateRoomAsync(roomRequest)).ReturnsAsync(addedRoom);

        // Act
        var result = await _controller.Add(roomRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
        Assert.True(apiResponse.Success);
        Assert.Equal(200, apiResponse.Status);
        Assert.Equal("Room added successfully", apiResponse.Message);
        Assert.Equal(addedRoom, apiResponse.Data);
    }

    [Fact]
    public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
    {
        // Arrange
        var roomRequest = new RoomRequest { Name = "Room A" };
        _roomServiceMock.Setup(service => service.CreateRoomAsync(roomRequest)).ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.Add(roomRequest);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
        Assert.False(apiResponse.Success);
        Assert.Equal(500, apiResponse.Status);
        Assert.Equal("Unexpected error", apiResponse.Message);
    }

    [Fact]
    public async Task Update_WhenRoomExists_ShouldReturnOk()
    {
        // Arrange
        var roomId = 1;
        var roomRequest = new RoomRequest { Name = "Updated Room" };
        var updatedRoom = new RoomResponse { Id = roomId, Name = "Updated Room" };

        _roomServiceMock.Setup(service => service.UpdateRoomAsync(roomId, roomRequest)).ReturnsAsync(updatedRoom);

        // Act
        var result = await _controller.Update(roomId, roomRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
        Assert.True(apiResponse.Success);
        Assert.Equal(200, apiResponse.Status);
        Assert.Equal("Room updated successfully", apiResponse.Message);
        Assert.Equal(updatedRoom, apiResponse.Data);
    }

    [Fact]
    public async Task Update_WhenRoomDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var roomId = 1;
        var roomRequest = new RoomRequest { Name = "Updated Room" };
        _roomServiceMock.Setup(service => service.UpdateRoomAsync(roomId, roomRequest)).ThrowsAsync(new NotFoundException("Room not found"));

        // Act
        var result = await _controller.Update(roomId, roomRequest);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
        Assert.False(apiResponse.Success);
        Assert.Equal(404, apiResponse.Status);
        Assert.Equal("Room not found", apiResponse.Message);
    }

    [Fact]
    public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
    {
        // Arrange
        var roomId = 1;
        var roomRequest = new RoomRequest { Name = "Updated Room" };
        _roomServiceMock.Setup(service => service.UpdateRoomAsync(roomId, roomRequest)).ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.Update(roomId, roomRequest);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
        Assert.False(apiResponse.Success);
        Assert.Equal(500, apiResponse.Status);
        Assert.Equal("Unexpected error", apiResponse.Message);
    }

    [Fact]
    public async Task Delete_WhenRoomExists_ShouldReturnOk()
    {
        // Arrange
        var roomId = 1;
        var deletedRoom = new Room { Id = roomId, Name = "Room A" };

        _roomServiceMock.Setup(service => service.DeleteRoomAsync(roomId)).ReturnsAsync(deletedRoom);

        // Act
        var result = await _controller.Delete(roomId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
        Assert.True(apiResponse.Success);
        Assert.Equal(200, apiResponse.Status);
        Assert.Equal("Room deleted successfully", apiResponse.Message);
        Assert.Equal(deletedRoom, apiResponse.Data);
    }

    [Fact]
    public async Task Delete_WhenRoomDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var roomId = 1;
        _roomServiceMock.Setup(service => service.DeleteRoomAsync(roomId)).ThrowsAsync(new NotFoundException("Room not found"));

        // Act
        var result = await _controller.Delete(roomId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
        Assert.False(apiResponse.Success);
        Assert.Equal(404, apiResponse.Status);
        Assert.Equal("Room not found", apiResponse.Message);
    }

    [Fact]
    public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
    {
        // Arrange
        var roomId = 1;
        _roomServiceMock.Setup(service => service.DeleteRoomAsync(roomId)).ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.Delete(roomId);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
        Assert.False(apiResponse.Success);
        Assert.Equal(500, apiResponse.Status);
        Assert.Equal("Unexpected error", apiResponse.Message);
    }
}