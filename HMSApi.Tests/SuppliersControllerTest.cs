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
    public class SuppliersControllerTest
    {
        private readonly Mock<ISupplierService> _supplierServiceMock;
        private readonly Mock<ILogger<SuppliersController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly SuppliersController _controller;

        public SuppliersControllerTest()
        {
            _supplierServiceMock = new Mock<ISupplierService>();
            _loggerMock = new Mock<ILogger<SuppliersController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new SuppliersController(_supplierServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllSuppliers_WhenSuppliersExist_ShouldReturnOk()
        {
            // Arrange
            var suppliers = new List<SupplierResponse>
            {
                new SupplierResponse { Id = 1, Name = "Supplier A" },
                new SupplierResponse { Id = 2, Name = "Supplier B" }
            };

            _supplierServiceMock.Setup(service => service.GetAllSuppliersAsync()).ReturnsAsync(suppliers);

            // Act
            var result = await _controller.GetAllSuppliers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<SupplierResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Suppliers fetched successfully", apiResponse.Message);
            Assert.Equal(suppliers, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllSuppliers_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _supplierServiceMock.Setup(service => service.GetAllSuppliersAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllSuppliers();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetSupplierById_WhenSupplierExists_ShouldReturnOk()
        {
            // Arrange
            var supplierId = 1;
            var supplier = new SupplierResponse { Id = supplierId, Name = "Supplier A" };

            _supplierServiceMock.Setup(service => service.GetSupplierByIdAsync(supplierId)).ReturnsAsync(supplier);

            // Act
            var result = await _controller.GetSupplierById(supplierId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Supplier fetched successfully", apiResponse.Message);
            Assert.Equal(supplier, apiResponse.Data);
        }

        [Fact]
        public async Task GetSupplierById_WhenSupplierDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var supplierId = 1;
            _supplierServiceMock.Setup(service => service.GetSupplierByIdAsync(supplierId)).ThrowsAsync(new NotFoundException("Supplier not found"));

            // Act
            var result = await _controller.GetSupplierById(supplierId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Supplier not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenSupplierIsValid_ShouldReturnOk()
        {
            // Arrange
            var supplierRequest = new SupplierRequest { Name = "Supplier A" };
            var addedSupplier = new SupplierResponse { Id = 1, Name = "Supplier A" };

            _supplierServiceMock.Setup(service => service.CreateSupplierAsync(supplierRequest)).ReturnsAsync(addedSupplier);

            // Act
            var result = await _controller.Add(supplierRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Supplier added successfully", apiResponse.Message);
            Assert.Equal(addedSupplier, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var supplierRequest = new SupplierRequest { Name = "Supplier A" };
            _supplierServiceMock.Setup(service => service.CreateSupplierAsync(supplierRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(supplierRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenSupplierExists_ShouldReturnOk()
        {
            // Arrange
            var supplierId = 1;
            var supplierRequest = new SupplierRequest { Name = "Updated Supplier" };
            var updatedSupplier = new SupplierResponse { Id = supplierId, Name = "Updated Supplier" };

            _supplierServiceMock.Setup(service => service.UpdateSupplierAsync(supplierId, supplierRequest)).ReturnsAsync(updatedSupplier);

            // Act
            var result = await _controller.Update(supplierId, supplierRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Supplier updated successfully", apiResponse.Message);
            Assert.Equal(updatedSupplier, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenSupplierDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var supplierId = 1;
            var supplierRequest = new SupplierRequest { Name = "Updated Supplier" };
            _supplierServiceMock.Setup(service => service.UpdateSupplierAsync(supplierId, supplierRequest)).ThrowsAsync(new NotFoundException("Supplier not found"));

            // Act
            var result = await _controller.Update(supplierId, supplierRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Supplier not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var supplierId = 1;
            var supplierRequest = new SupplierRequest { Name = "Updated Supplier" };
            _supplierServiceMock.Setup(service => service.UpdateSupplierAsync(supplierId, supplierRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(supplierId, supplierRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenSupplierExists_ShouldReturnOk()
        {
            // Arrange
            var supplierId = 1;
            var deletedSupplier = new Supplier { Id = supplierId, Name = "Supplier A" };

            _supplierServiceMock.Setup(service => service.DeleteSupplierAsync(supplierId)).ReturnsAsync(deletedSupplier);

            // Act
            var result = await _controller.Delete(supplierId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Supplier deleted successfully", apiResponse.Message);
            Assert.Equal(deletedSupplier, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenSupplierDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var supplierId = 1;
            _supplierServiceMock.Setup(service => service.DeleteSupplierAsync(supplierId)).ThrowsAsync(new NotFoundException("Supplier not found"));

            // Act
            var result = await _controller.Delete(supplierId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Supplier not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var supplierId = 1;
            _supplierServiceMock.Setup(service => service.DeleteSupplierAsync(supplierId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(supplierId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
