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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HMSApi.Tests
{
    public class PharmaciesControllerTest
    {
        private readonly Mock<IPharmacyService> _pharmacyServiceMock;
        private readonly Mock<ILogger<PharmaciesController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly PharmaciesController _controller;

        public PharmaciesControllerTest()
        {
            _pharmacyServiceMock = new Mock<IPharmacyService>();
            _loggerMock = new Mock<ILogger<PharmaciesController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new PharmaciesController(_pharmacyServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllPharmacies_WhenPharmaciesExist_ShouldReturnOk()
        {
            // Arrange
            var pharmacies = new List<PharmacyResponse>
            {
                new PharmacyResponse { Id = 1, Name = "Pharmacy A" },
                new PharmacyResponse { Id = 2, Name = "Pharmacy B" }
            };

            _pharmacyServiceMock.Setup(service => service.GetAllPharmaciesAsync()).ReturnsAsync(pharmacies);

            // Act
            var result = await _controller.GetAllPharmacies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<PharmacyResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Pharmacies fetched successfully", apiResponse.Message);
            Assert.Equal(pharmacies, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllPharmacies_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _pharmacyServiceMock.Setup(service => service.GetAllPharmaciesAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllPharmacies();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetPharmacyById_WhenPharmacyExists_ShouldReturnOk()
        {
            // Arrange
            var pharmacyId = 1;
            var pharmacy = new PharmacyResponse { Id = pharmacyId, Name = "Pharmacy A" };

            _pharmacyServiceMock.Setup(service => service.GetPharmacyByIdAsync(pharmacyId)).ReturnsAsync(pharmacy);

            // Act
            var result = await _controller.GetPharmacyById(pharmacyId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Pharmacy fetched successfully", apiResponse.Message);
            Assert.Equal(pharmacy, apiResponse.Data);
        }

        [Fact]
        public async Task GetPharmacyById_WhenPharmacyDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var pharmacyId = 1;
            _pharmacyServiceMock.Setup(service => service.GetPharmacyByIdAsync(pharmacyId)).ThrowsAsync(new NotFoundException("Pharmacy not found"));

            // Act
            var result = await _controller.GetPharmacyById(pharmacyId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Pharmacy not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenPharmacyIsValid_ShouldReturnOk()
        {
            // Arrange
            var pharmacyRequest = new PharmacyRequest { Name = "Pharmacy A" };
            var addedPharmacy = new PharmacyResponse { Id = 1, Name = "Pharmacy A" };

            _pharmacyServiceMock.Setup(service => service.CreatePharmacyAsync(pharmacyRequest)).ReturnsAsync(addedPharmacy);

            // Act
            var result = await _controller.Add(pharmacyRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Pharmacy added successfully", apiResponse.Message);
            Assert.Equal(addedPharmacy, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var pharmacyRequest = new PharmacyRequest { Name = "Pharmacy A" };
            _pharmacyServiceMock.Setup(service => service.CreatePharmacyAsync(pharmacyRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(pharmacyRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
