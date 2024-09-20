using AutoMapper;
using BusinessLayer.Requests;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedClasses.Exceptions;
using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FloorsController : ControllerBase
    {
        private readonly IFloorService floorService;
        private readonly ILogger<FloorsController> logger;
        private readonly IMapper mapper;

        public FloorsController(IFloorService floorService, ILogger<FloorsController> logger, IMapper mapper)
        {
            this.floorService = floorService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllFloors()
        {
            try
            {
                var floors = await floorService.GetAllFloorsAsync();
                var floorResponses = mapper.Map<List<FloorResponse>>(floors);
                return Ok(ApiResponseFactory.Create(floorResponses, "Floors fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while fetching floors. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> GetFloorById(int id)
        {
            try
            {
                var floor = await floorService.GetFloorByIdAsync(id);
                var floorResponse = mapper.Map<FloorResponse>(floor);
                return Ok(ApiResponseFactory.Create(floorResponse, "Floor fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Floor with ID: {Id} not found.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while fetching floor with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> Add(FloorRequest request)
        {
            try
            {
                var addedFloor = await floorService.CreateFloorAsync(request);
                var floorResponse = mapper.Map<FloorResponse>(addedFloor);
                return Ok(ApiResponseFactory.Create(floorResponse, "Floor added successfully", 200, true));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while adding floor. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, FloorRequest request)
        {
            try
            {
                var updatedFloor = await floorService.UpdateFloorAsync(id, request);
                var floorResponse = mapper.Map<FloorResponse>(updatedFloor);
                return Ok(ApiResponseFactory.Create(floorResponse, "Floor updated successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Floor with ID: {Id} not found for update.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while updating floor with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                var deletedFloor = await floorService.DeleteFloorAsync(id);
                return Ok(ApiResponseFactory.Create(deletedFloor, "Floor deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Floor with ID: {Id} not found for deletion.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while deleting floor with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{floorId:int}/rooms")]
        public async Task<ActionResult<IApiResponse>> GetRoomsByFloorId(int floorId)
        {
            try
            {
                var rooms = await floorService.GetRoomsByFloorIdAsync(floorId);
                return Ok(ApiResponseFactory.Create(rooms, "Rooms fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Rooms not found for floor with ID: {FloorId}.", floorId);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while fetching rooms for floor with ID: {FloorId}. Log message: {logMessage}", floorId, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }
    }
}
