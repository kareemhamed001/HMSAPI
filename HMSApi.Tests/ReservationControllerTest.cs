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
    public class ReservationControllerTest
    {
        private readonly Mock<IReservationService> _reservationServiceMock;
        private readonly Mock<ILogger<ReservationController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly ReservationController _controller;

        public ReservationControllerTest()
        {
            _reservationServiceMock = new Mock<IReservationService>();
            _loggerMock = new Mock<ILogger<ReservationController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new ReservationController(_reservationServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllReservations_WhenReservationsExist_ShouldReturnOk()
        {
            // Arrange
            var reservations = new List<ReservationResponse>
            {
                new ReservationResponse { Id = 1,PatientId=1},
                new ReservationResponse { Id = 1,PatientId=1},
            };

            _reservationServiceMock.Setup(service => service.GetAllReservationsAsync()).ReturnsAsync(reservations);

            // Act
            var result = await _controller.GetAllReservations();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<ReservationResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Reservations fetched successfully", apiResponse.Message);
            Assert.Equal(reservations, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllReservations_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _reservationServiceMock.Setup(service => service.GetAllReservationsAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllReservations();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }


        [Fact]
        public async Task GetReservationById_WhenReservationDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var reservationId = 1;
            _reservationServiceMock.Setup(service => service.GetReservationByIdAsync(reservationId)).ThrowsAsync(new NotFoundException("Reservation not found"));

            // Act
            var result = await _controller.GetReservationById(reservationId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Reservation not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenReservationIsValid_ShouldReturnOk()
        {
            // Arrange
            var reservationRequest = new ReservationRequest { PatientId = 1 };
            var addedReservation = new ReservationResponse { PatientId = 1 };

            _reservationServiceMock.Setup(service => service.CreateReservationAsync(reservationRequest)).ReturnsAsync(addedReservation);

            // Act
            var result = await _controller.Add(reservationRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Reservation added successfully", apiResponse.Message);
            Assert.Equal(addedReservation, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var reservationRequest = new ReservationRequest { PatientId = 1 };
            _reservationServiceMock.Setup(service => service.CreateReservationAsync(reservationRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(reservationRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenReservationExists_ShouldReturnOk()
        {
            // Arrange
            var reservationId = 1;
            var reservationRequest = new ReservationRequest { PatientId = 1 };
            var updatedReservation = new ReservationResponse { PatientId = 1 }; 

            _reservationServiceMock.Setup(service => service.UpdateReservationAsync(reservationId, reservationRequest)).ReturnsAsync(updatedReservation);

            // Act
            var result = await _controller.Update(reservationId, reservationRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Reservation updated successfully", apiResponse.Message);
            Assert.Equal(updatedReservation, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenReservationDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var reservationId = 1;
            var reservationRequest = new ReservationRequest { PatientId = 1 };
            _reservationServiceMock.Setup(service => service.UpdateReservationAsync(reservationId, reservationRequest)).ThrowsAsync(new NotFoundException("Reservation not found"));

            // Act
            var result = await _controller.Update(reservationId, reservationRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Reservation not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var reservationId = 1;
            var reservationRequest = new ReservationRequest { PatientId = 1 };
            _reservationServiceMock.Setup(service => service.UpdateReservationAsync(reservationId, reservationRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(reservationId, reservationRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenReservationExists_ShouldReturnOk()
        {
            // Arrange
            var reservationId = 1;
            var deletedReservation = new Reservation { PatientId = 1 };

            _reservationServiceMock.Setup(service => service.DeleteReservationAsync(reservationId)).ReturnsAsync(deletedReservation);

            // Act
            var result = await _controller.Delete(reservationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Reservation deleted successfully", apiResponse.Message);
            Assert.Equal(deletedReservation, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenReservationDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var reservationId = 1;
            _reservationServiceMock.Setup(service => service.DeleteReservationAsync(reservationId)).ThrowsAsync(new NotFoundException("Reservation not found"));

            // Act
            var result = await _controller.Delete(reservationId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Reservation not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var reservationId = 1;
            _reservationServiceMock.Setup(service => service.DeleteReservationAsync(reservationId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(reservationId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
