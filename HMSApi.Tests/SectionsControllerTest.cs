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
    public class SectionsControllerTest
    {
        private readonly Mock<ISectionService> _sectionServiceMock;
        private readonly Mock<ILogger<SectionsController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly SectionsController _controller;

        public SectionsControllerTest()
        {
            _sectionServiceMock = new Mock<ISectionService>();
            _loggerMock = new Mock<ILogger<SectionsController>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
            _controller = new SectionsController(_sectionServiceMock.Object, _loggerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllSections_WhenSectionsExist_ShouldReturnOk()
        {
            // Arrange
            var sections = new List<SectionResponse>
            {
                new SectionResponse { Id = 1, Name = "Section A" },
                new SectionResponse { Id = 2, Name = "Section B" }
            };

            _sectionServiceMock.Setup(service => service.GetAllSectionsAsync()).ReturnsAsync(sections);

            // Act
            var result = await _controller.GetAllSections();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseListStrategy<SectionResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Sections fetched successfully", apiResponse.Message);
            Assert.Equal(sections, apiResponse.Data);
        }

        [Fact]
        public async Task GetAllSections_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            _sectionServiceMock.Setup(service => service.GetAllSectionsAsync()).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllSections();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task GetSectionById_WhenSectionExists_ShouldReturnOk()
        {
            // Arrange
            var sectionId = 1;
            var section = new SectionResponse { Id = sectionId, Name = "Section A" };

            _sectionServiceMock.Setup(service => service.GetSectionByIdAsync(sectionId)).ReturnsAsync(section);

            // Act
            var result = await _controller.GetSectionById(sectionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Section fetched successfully", apiResponse.Message);
            Assert.Equal(section, apiResponse.Data);
        }

        [Fact]
        public async Task GetSectionById_WhenSectionDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var sectionId = 1;
            _sectionServiceMock.Setup(service => service.GetSectionByIdAsync(sectionId)).ThrowsAsync(new NotFoundException("Section not found"));

            // Act
            var result = await _controller.GetSectionById(sectionId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Section not found", apiResponse.Message);
        }

        [Fact]
        public async Task Add_WhenSectionIsValid_ShouldReturnOk()
        {
            // Arrange
            var sectionRequest = new SectionRequest { Name = "Section A" };
            var addedSection = new SectionResponse { Id = 1, Name = "Section A" };

            _sectionServiceMock.Setup(service => service.CreateSectionAsync(sectionRequest)).ReturnsAsync(addedSection);

            // Act
            var result = await _controller.Add(sectionRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Section added successfully", apiResponse.Message);
            Assert.Equal(addedSection, apiResponse.Data);
        }

        [Fact]
        public async Task Add_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var sectionRequest = new SectionRequest { Name = "Section A" };
            _sectionServiceMock.Setup(service => service.CreateSectionAsync(sectionRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Add(sectionRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenSectionExists_ShouldReturnOk()
        {
            // Arrange
            var sectionId = 1;
            var sectionRequest = new SectionRequest { Name = "Updated Section" };
            var updatedSection = new SectionResponse { Id = sectionId, Name = "Updated Section" };

            _sectionServiceMock.Setup(service => service.UpdateSectionAsync(sectionId, sectionRequest)).ReturnsAsync(updatedSection);

            // Act
            var result = await _controller.Update(sectionId, sectionRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Section updated successfully", apiResponse.Message);
            Assert.Equal(updatedSection, apiResponse.Data);
        }

        [Fact]
        public async Task Update_WhenSectionDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var sectionId = 1;
            var sectionRequest = new SectionRequest { Name = "Updated Section" };
            _sectionServiceMock.Setup(service => service.UpdateSectionAsync(sectionId, sectionRequest)).ThrowsAsync(new NotFoundException("Section not found"));

            // Act
            var result = await _controller.Update(sectionId, sectionRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Section not found", apiResponse.Message);
        }

        [Fact]
        public async Task Update_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var sectionId = 1;
            var sectionRequest = new SectionRequest { Name = "Updated Section" };
            _sectionServiceMock.Setup(service => service.UpdateSectionAsync(sectionId, sectionRequest)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(sectionId, sectionRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenSectionExists_ShouldReturnOk()
        {
            // Arrange
            var sectionId = 1;
            var deletedSection = new Section { Id = sectionId, Name = "Section A" };

            _sectionServiceMock.Setup(service => service.DeleteSectionAsync(sectionId)).ReturnsAsync(deletedSection);

            // Act
            var result = await _controller.Delete(sectionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseSingleStrategy>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(200, apiResponse.Status);
            Assert.Equal("Section deleted successfully", apiResponse.Message);
            Assert.Equal(deletedSection, apiResponse.Data);
        }

        [Fact]
        public async Task Delete_WhenSectionDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var sectionId = 1;
            _sectionServiceMock.Setup(service => service.DeleteSectionAsync(sectionId)).ThrowsAsync(new NotFoundException("Section not found"));

            // Act
            var result = await _controller.Delete(sectionId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(404, apiResponse.Status);
            Assert.Equal("Section not found", apiResponse.Message);
        }

        [Fact]
        public async Task Delete_WhenExceptionOccurs_ShouldReturnInternalServerError()
        {
            // Arrange
            var sectionId = 1;
            _sectionServiceMock.Setup(service => service.DeleteSectionAsync(sectionId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Delete(sectionId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponseBase>(statusCodeResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(500, apiResponse.Status);
            Assert.Equal("Unexpected error", apiResponse.Message);
        }
    }
}
