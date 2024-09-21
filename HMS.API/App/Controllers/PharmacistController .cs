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
    public class PharmacistController : ControllerBase
    {
        private readonly IPharmacistService _pharmacistService;
        private readonly ILogger<PharmacistController> _logger;
        private readonly IMapper _mapper;

        public PharmacistController(IPharmacistService pharmacistService, ILogger<PharmacistController> logger, IMapper mapper)
        {
            _pharmacistService = pharmacistService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllPharmacists()
        {
            try
            {
                var pharmacists = await _pharmacistService.GetAllPharmacistsAsync();
                return Ok(ApiResponseFactory.Create(pharmacists, "Pharmacists fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching pharmacists.");
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> GetPharmacistById(int id)
        {
            try
            {
                var pharmacist = await _pharmacistService.GetPharmacistByIdAsync(id);
                return Ok(ApiResponseFactory.Create(pharmacist, "Pharmacist fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Pharmacist with ID: {Id} not found.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching pharmacist with ID: {Id}", id);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> Add(PharmacistRequest request)
        {
            try
            {
                var addedPharmacist = await _pharmacistService.CreatePharmacistAsync(request);
                return Ok(ApiResponseFactory.Create(addedPharmacist, "Pharmacist added successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while adding pharmacist.");
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, PharmacistRequest request)
        {
            try
            {
                var updatedPharmacist = await _pharmacistService.UpdatePharmacistAsync(id, request);
                return Ok(ApiResponseFactory.Create(updatedPharmacist, "Pharmacist updated successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Pharmacist with ID: {Id} not found for update.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while updating pharmacist with ID: {Id}", id);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                var deletedPharmacist = await _pharmacistService.DeletePharmacistAsync(id);
                return Ok(ApiResponseFactory.Create(deletedPharmacist, "Pharmacist deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Pharmacist with ID: {Id} not found for deletion.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while deleting pharmacist with ID: {Id}", id);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }
    }
}
