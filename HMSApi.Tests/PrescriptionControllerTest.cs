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
    public class PrescriptionControllerTest
    {
        private readonly Mock<IPrescriptionService> _prescriptionServiceMock;
        private readonly Mock<ILogger<PrescriptionController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly PrescriptionController _controller;

        public PrescriptionControllerTest()
        {
            _prescriptionServiceMock = new Mock<IPrescriptionService>();
            _loggerMock = new Mock<ILogger<PrescriptionController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new PrescriptionController(_prescriptionServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllPrescriptions_WhenPrescriptionsExist_ShouldReturnOk()
        {
            // Arrange
            var prescriptions = new List<PrescriptionResponse>
            {
                new PrescriptionResponse{ DoctorId = 1 },
                new PrescriptionResponse{ DoctorId = 1 }
            };

            _prescriptionServiceMock.Setup(service => service.GetAllPrescriptionsAsync()).ReturnsAsync(prescriptions);

            // Act
            var result = await _controller.GetAllPrescriptions();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<PrescriptionResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Prescriptions fetched successfully", apiResponse.Message);
            Assert.Equal(prescriptions, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllPrescriptions_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _prescriptionServiceMock.Setup(service => service.GetAllPrescriptionsAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllPrescriptions();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetPrescriptionById_WhenPrescriptionExists_ShouldReturnOk()
        {
            // Arrange
            var prescriptionId = 1;
            var prescription = new PrescriptionResponse { DoctorId = 1 };

            _prescriptionServiceMock.Setup(service => service.GetPrescriptionByIdAsync(prescriptionId)).ReturnsAsync(prescription);

            // Act
            var result = await _controller.GetPrescriptionById(prescriptionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Prescription fetched successfully", apiResponse.Message);
            Assert.Equal(prescription, apiResponse.Data);
        }

        [Fact]
        public async Task GetPrescriptionById_WhenPrescriptionDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var prescriptionId = 1;
            _prescriptionServiceMock.Setup(service => service.GetPrescriptionByIdAsync(prescriptionId)).ThrowsAsync(new NotFoundException("Prescription not found"));

            // Act
            var result = await _controller.GetPrescriptionById(prescriptionId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Prescription not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenPrescriptionIsValid_ShouldReturnOk()
        {
            // Arrange
            var prescriptionRequest = new PrescriptionRequest { DoctorId = 1 };
            var addedPrescription = new PrescriptionResponse { DoctorId = 1 };

            _prescriptionServiceMock.Setup(service => service.CreatePrescriptionAsync(prescriptionRequest)).ReturnsAsync(addedPrescription);

            // Act
            var result = await _controller.Add(prescriptionRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Prescription added successfully", apiResponse.Message);
            Assert.Equal(addedPrescription, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var prescriptionRequest = new PrescriptionRequest { DoctorId = 1 };
            _prescriptionServiceMock.Setup(service => service.CreatePrescriptionAsync(prescriptionRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(prescriptionRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenPrescriptionExists_ShouldReturnOk()
        {
            // Arrange
            var prescriptionId = 1;
            var prescriptionRequest = new PrescriptionRequest { DoctorId = 1 };
            var updatedPrescription = new PrescriptionResponse { DoctorId = 1 };

            _prescriptionServiceMock.Setup(service => service.UpdatePrescriptionAsync(prescriptionId, prescriptionRequest)).ReturnsAsync(updatedPrescription);

            // Act
            var result = await _controller.Update(prescriptionId, prescriptionRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Prescription updated successfully", apiResponse.Message);
            Assert.Equal(updatedPrescription, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenPrescriptionDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var prescriptionId = 1;
            var prescriptionRequest = new PrescriptionRequest { DoctorId = 1 };
            _prescriptionServiceMock.Setup(service => service.UpdatePrescriptionAsync(prescriptionId, prescriptionRequest)).ThrowsAsync(new NotFoundException("Prescription not found"));

            // Act
            var result = await _controller.Update(prescriptionId, prescriptionRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Prescription not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var prescriptionId = 1;
            var prescriptionRequest = new PrescriptionRequest { DoctorId = 1 };
            _prescriptionServiceMock.Setup(service => service.UpdatePrescriptionAsync(prescriptionId, prescriptionRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(prescriptionId, prescriptionRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenPrescriptionExists_ShouldReturnOk()
        {
            // Arrange
            var prescriptionId = 1;
            var deletedPrescription = new Prescription { DoctorId = 1 };

            _prescriptionServiceMock.Setup(service => service.DeletePrescriptionAsync(prescriptionId)).ReturnsAsync(deletedPrescription);

            // Act
            var result = await _controller.Delete(prescriptionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Prescription deleted successfully", apiResponse.Message);
            Assert.Equal(deletedPrescription, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenPrescriptionDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var prescriptionId = 1;
            _prescriptionServiceMock.Setup(service => service.DeletePrescriptionAsync(prescriptionId)).ThrowsAsync(new NotFoundException("Prescription not found"));

            // Act
            var result = await _controller.Delete(prescriptionId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Prescription not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var prescriptionId = 1;
            _prescriptionServiceMock.Setup(service => service.DeletePrescriptionAsync(prescriptionId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(prescriptionId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
