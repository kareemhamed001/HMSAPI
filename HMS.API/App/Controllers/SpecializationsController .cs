using AutoMapper;
using BusinessLayer.Requests;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedClasses.Exceptions;
using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationsController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;
        private readonly ILogger<SpecializationsController> _logger;
        private readonly IMapper _mapper;

        public SpecializationsController(ISpecializationService specializationService, ILogger<SpecializationsController> logger, IMapper mapper)
        {
            _specializationService = specializationService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllSpecializations()
        {
            try
            {
                var specializations = await _specializationService.GetAllSpecializationsAsync();
                return Ok(ApiResponseFactory.Create(specializations, "Specializations fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching specializations. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> GetSpecializationById(int id)
        {
            try
            {
                var specialization = await _specializationService.GetSpecializationByIdAsync(id);
                return Ok(ApiResponseFactory.Create(specialization, "Specialization fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Specialization with ID: {Id} not found.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching specialization with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> Add([FromForm]SpecializationRequest request)
        {
            try
            {
                var addedSpecialization = await _specializationService.CreateSpecializationAsync(request);
                return Ok(ApiResponseFactory.Create(addedSpecialization, "Specialization added successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while adding specialization. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, SpecializationRequest request)
        {
            try
            {
                var updatedSpecialization = await _specializationService.UpdateSpecializationAsync(id, request);
                return Ok(ApiResponseFactory.Create(updatedSpecialization, "Specialization updated successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Specialization with ID: {Id} not found for update.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while updating specialization with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                var deletedSpecialization = await _specializationService.DeleteSpecializationAsync(id);
                return Ok(ApiResponseFactory.Create(deletedSpecialization, "Specialization deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Specialization with ID: {Id} not found for deletion.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while deleting specialization with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }
    }
}
