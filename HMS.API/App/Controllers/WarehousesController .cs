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
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        private readonly ILogger<WarehousesController> _logger;
        private readonly IMapper _mapper;

        public WarehousesController(IWarehouseService warehouseService, ILogger<WarehousesController> logger, IMapper mapper)
        {
            _warehouseService = warehouseService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllWarehouses()
        {
            try
            {
                var warehouses = await _warehouseService.GetAllWarehousesAsync();
                return Ok(ApiResponseFactory.Create(warehouses, "Warehouses fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching warehouses. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> GetWarehouseById(int id)
        {
            try
            {
                var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);
                return Ok(ApiResponseFactory.Create(warehouse, "Warehouse fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Warehouse with ID: {Id} not found.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while fetching warehouse with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> Add(WarehouseRequest request)
        {
            try
            {
                var addedWarehouse = await _warehouseService.CreateWarehouseAsync(request);
                return Ok(ApiResponseFactory.Create(addedWarehouse, "Warehouse added successfully", 200, true));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while adding warehouse. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, WarehouseRequest request)
        {
            try
            {
                var updatedWarehouse = await _warehouseService.UpdateWarehouseAsync(id, request);
                return Ok(ApiResponseFactory.Create(updatedWarehouse, "Warehouse updated successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Warehouse with ID: {Id} not found for update.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while updating warehouse with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                var deletedWarehouse = await _warehouseService.DeleteWarehouseAsync(id);
                return Ok(ApiResponseFactory.Create(deletedWarehouse, "Warehouse deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Warehouse with ID: {Id} not found for deletion.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while deleting warehouse with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }
    }
}
