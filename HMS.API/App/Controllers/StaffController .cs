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
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly ILogger<StaffController> _logger;
        private readonly IMapper _mapper;

        public StaffController(IStaffService staffService, ILogger<StaffController> logger, IMapper mapper)
        {
            _staffService = staffService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllStaff()
        {
            try
            {
                var staffMembers = await _staffService.GetAllStaffAsync();
                return Ok(ApiResponseFactory.Create(staffMembers, "Staff fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching staff.");
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> GetStaffById(int id)
        {
            try
            {
                var staff = await _staffService.GetStaffByIdAsync(id);
                return Ok(ApiResponseFactory.Create(staff, "Staff fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Staff with ID: {Id} not found.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching staff with ID: {Id}", id);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> Add(StaffRequest request)
        {
            try
            {
                var addedStaff = await _staffService.CreateStaffAsync(request);
                return Ok(ApiResponseFactory.Create(addedStaff, "Staff added successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while adding staff.");
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, StaffRequest request)
        {
            try
            {
                var updatedStaff = await _staffService.UpdateStaffAsync(id, request);
                return Ok(ApiResponseFactory.Create(updatedStaff, "Staff updated successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Staff with ID: {Id} not found for update.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while updating staff with ID: {Id}", id);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                var deletedStaff = await _staffService.DeleteStaffAsync(id);
                return Ok(ApiResponseFactory.Create(deletedStaff, "Staff deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Staff with ID: {Id} not found for deletion.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while deleting staff with ID: {Id}", id);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }
    }
}
