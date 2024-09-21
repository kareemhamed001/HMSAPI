using AutoMapper;
using BusinessLayer.Requests;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedClasses.Exceptions;
using SharedClasses.Responses;
using System.Threading.Tasks;

namespace LMSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PharmaciesController : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly ILogger<PharmaciesController> _logger;
        private readonly IMapper _mapper;

        public PharmaciesController(IPharmacyService pharmacyService, ILogger<PharmaciesController> logger, IMapper mapper)
        {
            _pharmacyService = pharmacyService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllPharmacies()
        {
            try
            {
                var pharmacies = await _pharmacyService.GetAllPharmaciesAsync();
                return Ok(ApiResponseFactory.Create(pharmacies, "Pharmacies fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching pharmacies. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> GetPharmacyById(int id)
        {
            try
            {
                var pharmacy = await _pharmacyService.GetPharmacyByIdAsync(id);
                return Ok(ApiResponseFactory.Create(pharmacy, "Pharmacy fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Pharmacy with ID: {Id} not found.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching pharmacy with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> Add(PharmacyRequest request)
        {
            try
            {
                var addedPharmacy = await _pharmacyService.CreatePharmacyAsync(request);
                return Ok(ApiResponseFactory.Create(addedPharmacy, "Pharmacy added successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while adding pharmacy. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, PharmacyRequest request)
        {
            try
            {
                var updatedPharmacy = await _pharmacyService.UpdatePharmacyAsync(id, request);
                return Ok(ApiResponseFactory.Create(updatedPharmacy, "Pharmacy updated successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Pharmacy with ID: {Id} not found for update.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while updating pharmacy with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                var deletedPharmacy = await _pharmacyService.DeletePharmacyAsync(id);
                return Ok(ApiResponseFactory.Create(deletedPharmacy, "Pharmacy deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Pharmacy with ID: {Id} not found for deletion.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while deleting pharmacy with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }
    }
}
