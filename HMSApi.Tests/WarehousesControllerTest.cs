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

namespace HMSApi.Tests
{
    public class WarehousesControllerTest
    {
        private readonly Mock<IWarehouseService> _warehouseServiceMock;
        private readonly Mock<ILogger<WarehousesController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly WarehousesController _controller;

        public WarehousesControllerTest()
        {
            _warehouseServiceMock = new Mock<IWarehouseService>();
            _loggerMock = new Mock<ILogger<WarehousesController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new WarehousesController(_warehouseServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllWarehouses_WhenWarehousesExist_ShouldReturnOk()
        {
            // Arrange
            var warehouses = new List<WarehouseResponse>
            {
                new WarehouseResponse { Id = 1, Name = "Warehouse A" },
                new WarehouseResponse { Id = 2, Name = "Warehouse B" }
            };

            _warehouseServiceMock.Setup(service => service.GetAllWarehousesAsync()).ReturnsAsync(warehouses);

            // Act
            var result = await _controller.GetAllWarehouses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<WarehouseResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Warehouses fetched successfully", apiResponse.Message);
            Assert.Equal(warehouses, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllWarehouses_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _warehouseServiceMock.Setup(service => service.GetAllWarehousesAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllWarehouses();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetWarehouseById_WhenWarehouseExists_ShouldReturnOk()
        {
            // Arrange
            var warehouseId = 1;
            var warehouse = new WarehouseResponse { Id = warehouseId, Name = "Warehouse A" };

            _warehouseServiceMock.Setup(service => service.GetWarehouseByIdAsync(warehouseId)).ReturnsAsync(warehouse);

            // Act
            var result = await _controller.GetWarehouseById(warehouseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Warehouse fetched successfully", apiResponse.Message);
            Assert.Equal(warehouse, apiResponse.Data);
        }

        [Fact]
        public async Task GetWarehouseById_WhenWarehouseDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var warehouseId = 1;
            _warehouseServiceMock.Setup(service => service.GetWarehouseByIdAsync(warehouseId)).ThrowsAsync(new NotFoundException("Warehouse not found"));

            // Act
            var result = await _controller.GetWarehouseById(warehouseId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Warehouse not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenWarehouseIsValid_ShouldReturnOk()
        {
            // Arrange
            var warehouseRequest = new WarehouseRequest { Name = "Warehouse A" };
            var addedWarehouse = new WarehouseResponse { Id = 1, Name = "Warehouse A" };

            _warehouseServiceMock.Setup(service => service.CreateWarehouseAsync(warehouseRequest)).ReturnsAsync(addedWarehouse);

            // Act
            var result = await _controller.Add(warehouseRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Warehouse added successfully", apiResponse.Message);
            Assert.Equal(addedWarehouse, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var warehouseRequest = new WarehouseRequest { Name = "Warehouse A" };
            _warehouseServiceMock.Setup(service => service.CreateWarehouseAsync(warehouseRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(warehouseRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenWarehouseExists_ShouldReturnOk()
        {
            // Arrange
            var warehouseId = 1;
            var warehouseRequest = new WarehouseRequest { Name = "Updated Warehouse" };
            var updatedWarehouse = new WarehouseResponse { Id = warehouseId, Name = "Updated Warehouse" };

            _warehouseServiceMock.Setup(service => service.UpdateWarehouseAsync(warehouseId, warehouseRequest)).ReturnsAsync(updatedWarehouse);

            // Act
            var result = await _controller.Update(warehouseId, warehouseRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Warehouse updated successfully", apiResponse.Message);
            Assert.Equal(updatedWarehouse, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenWarehouseDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var warehouseId = 1;
            var warehouseRequest = new WarehouseRequest { Name = "Updated Warehouse" };
            _warehouseServiceMock.Setup(service => service.UpdateWarehouseAsync(warehouseId, warehouseRequest)).ThrowsAsync(new NotFoundException("Warehouse not found"));

            // Act
            var result = await _controller.Update(warehouseId, warehouseRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Warehouse not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var warehouseId = 1;
            var warehouseRequest = new WarehouseRequest { Name = "Updated Warehouse" };
            _warehouseServiceMock.Setup(service => service.UpdateWarehouseAsync(warehouseId, warehouseRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(warehouseId, warehouseRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenWarehouseExists_ShouldReturnOk()
        {
            // Arrange
            var warehouseId = 1;
            var deletedWarehouse = new Warehouse { Id = warehouseId, Name = "Warehouse A" };

            _warehouseServiceMock.Setup(service => service.DeleteWarehouseAsync(warehouseId)).ReturnsAsync(deletedWarehouse);

            // Act
            var result = await _controller.Delete(warehouseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Warehouse deleted successfully", apiResponse.Message);
            Assert.Equal(deletedWarehouse, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenWarehouseDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var warehouseId = 1;
            _warehouseServiceMock.Setup(service => service.DeleteWarehouseAsync(warehouseId)).ThrowsAsync(new NotFoundException("Warehouse not found"));

            // Act
            var result = await _controller.Delete(warehouseId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Warehouse not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var warehouseId = 1;
            _warehouseServiceMock.Setup(service => service.DeleteWarehouseAsync(warehouseId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(warehouseId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
