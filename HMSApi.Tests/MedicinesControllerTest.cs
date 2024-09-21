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
    public class MedicinesControllerTest
    {
        private readonly Mock<IMedicineService> _medicineServiceMock;
        private readonly Mock<ILogger<MedicinesController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly MedicinesController _controller;

        public MedicinesControllerTest()
        {
            _medicineServiceMock = new Mock<IMedicineService>();
            _loggerMock = new Mock<ILogger<MedicinesController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new MedicinesController(_medicineServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllMedicines_WhenMedicinesExist_ShouldReturnOk()
        {
            // Arrange
            var medicines = new List<MedicineResponse>
            {
                new MedicineResponse { Id = 1, Name = "Medicine A" },
                new MedicineResponse { Id = 2, Name = "Medicine B" }
            };

            _medicineServiceMock.Setup(service => service.GetAllMedicinesAsync()).ReturnsAsync(medicines);

            // Act
            var result = await _controller.GetAllMedicines();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<MedicineResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Medicines fetched successfully", apiResponse.Message);
            Assert.Equal(medicines, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllMedicines_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _medicineServiceMock.Setup(service => service.GetAllMedicinesAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllMedicines();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetMedicineById_WhenMedicineExists_ShouldReturnOk()
        {
            // Arrange
            var medicineId = 1;
            var medicine = new MedicineResponse { Id = medicineId, Name = "Medicine A" };

            _medicineServiceMock.Setup(service => service.GetMedicineByIdAsync(medicineId)).ReturnsAsync(medicine);

            // Act
            var result = await _controller.GetMedicineById(medicineId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Medicine fetched successfully", apiResponse.Message);
            Assert.Equal(medicine, apiResponse.Data);
        }

        [Fact]
        public async Task GetMedicineById_WhenMedicineDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var medicineId = 1;
            _medicineServiceMock.Setup(service => service.GetMedicineByIdAsync(medicineId)).ThrowsAsync(new NotFoundException("Medicine not found"));

            // Act
            var result = await _controller.GetMedicineById(medicineId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Medicine not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenMedicineIsValid_ShouldReturnOk()
        {
            // Arrange
            var medicineRequest = new MedicineRequest { Name = "Medicine A" };
            var addedMedicine = new MedicineResponse { Id = 1, Name = "Medicine A" };

            _medicineServiceMock.Setup(service => service.CreateMedicineAsync(medicineRequest)).ReturnsAsync(addedMedicine);

            // Act
            var result = await _controller.Add(medicineRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Medicine added successfully", apiResponse.Message);
            Assert.Equal(addedMedicine, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var medicineRequest = new MedicineRequest { Name = "Medicine A" };
            _medicineServiceMock.Setup(service => service.CreateMedicineAsync(medicineRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(medicineRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenMedicineExists_ShouldReturnOk()
        {
            // Arrange
            var medicineId = 1;
            var medicineRequest = new MedicineRequest { Name = "Updated Medicine" };
            var updatedMedicine = new MedicineResponse { Id = medicineId, Name = "Updated Medicine" };

            _medicineServiceMock.Setup(service => service.UpdateMedicineAsync(medicineId, medicineRequest)).ReturnsAsync(updatedMedicine);

            // Act
            var result = await _controller.Update(medicineId, medicineRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Medicine updated successfully", apiResponse.Message);
            Assert.Equal(updatedMedicine, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenMedicineDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var medicineId = 1;
            var medicineRequest = new MedicineRequest { Name = "Updated Medicine" };
            _medicineServiceMock.Setup(service => service.UpdateMedicineAsync(medicineId, medicineRequest)).ThrowsAsync(new NotFoundException("Medicine not found"));

            // Act
            var result = await _controller.Update(medicineId, medicineRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Medicine not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var medicineId = 1;
            var medicineRequest = new MedicineRequest { Name = "Updated Medicine" };
            _medicineServiceMock.Setup(service => service.UpdateMedicineAsync(medicineId, medicineRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(medicineId, medicineRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenMedicineExists_ShouldReturnOk()
        {
            // Arrange
            var medicineId = 1;
            var deletedMedicine = new Medicine { Id = medicineId, Name = "Medicine A" };

            _medicineServiceMock.Setup(service => service.DeleteMedicineAsync(medicineId)).ReturnsAsync(deletedMedicine);

            // Act
            var result = await _controller.Delete(medicineId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Medicine deleted successfully", apiResponse.Message);
            Assert.Equal(deletedMedicine, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenMedicineDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var medicineId = 1;
            _medicineServiceMock.Setup(service => service.DeleteMedicineAsync(medicineId)).ThrowsAsync(new NotFoundException("Medicine not found"));

            // Act
            var result = await _controller.Delete(medicineId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Medicine not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var medicineId = 1;
            _medicineServiceMock.Setup(service => service.DeleteMedicineAsync(medicineId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(medicineId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
