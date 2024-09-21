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
    public class NursesController : ControllerBase
    {
        private readonly INurseService _nurseService;
        private readonly ILogger<NursesController> _logger;
        private readonly IMapper _mapper;

        public NursesController(INurseService nurseService, ILogger<NursesController> logger, IMapper mapper)
        {
            _nurseService = nurseService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllNurses()
        {
            try
            {
                var nurses = await _nurseService.GetAllNursesAsync();
                return Ok(ApiResponseFactory.Create(nurses, "Nurses fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching nurses.");
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> GetNurseById(int id)
        {
            try
            {
                var nurse = await _nurseService.GetNurseByIdAsync(id);
                return Ok(ApiResponseFactory.Create(nurse, "Nurse fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Nurse with ID: {Id} not found.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching nurse with ID: {Id}", id);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> Add(NurseRequest request)
        {
            try
            {
                var addedNurse = await _nurseService.CreateNurseAsync(request);
                return Ok(ApiResponseFactory.Create(addedNurse, "Nurse added successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while adding nurse.");
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, NurseRequest request)
        {
            try
            {
                var updatedNurse = await _nurseService.UpdateNurseAsync(id, request);
                return Ok(ApiResponseFactory.Create(updatedNurse, "Nurse updated successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Nurse with ID: {Id} not found for update.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while updating nurse with ID: {Id}", id);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                var deletedNurse = await _nurseService.DeleteNurseAsync(id);
                return Ok(ApiResponseFactory.Create(deletedNurse, "Nurse deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Nurse with ID: {Id} not found for deletion.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while deleting nurse with ID: {Id}", id);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }
    }
}
