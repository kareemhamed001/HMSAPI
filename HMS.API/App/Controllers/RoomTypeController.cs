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
    public class RoomTypesController : ControllerBase
    {
        private readonly IRoomTypeService roomTypeService;
        private readonly ILogger<RoomTypesController> logger;
        private readonly IMapper mapper;

        public RoomTypesController(IRoomTypeService roomTypeService, ILogger<RoomTypesController> logger, IMapper mapper)
        {
            this.roomTypeService = roomTypeService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllRoomTypes()
        {
            try
            {
                var roomTypes = await roomTypeService.GetAllRoomTypesAsync();
                return Ok(ApiResponseFactory.Create(roomTypes, "Room types fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while fetching room types. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> GetRoomTypeById(int id)
        {
            try
            {
                var roomType = await roomTypeService.GetRoomTypeByIdAsync(id);
                return Ok(ApiResponseFactory.Create(roomType, "Room type fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Room type with ID: {Id} not found.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while fetching room type with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> Add(RoomTypeRequest request)
        {
            try
            {
                var addedRoomType = await roomTypeService.CreateRoomTypeAsync(request);
                return Ok(ApiResponseFactory.Create(addedRoomType, "Room type added successfully", 200, true));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while adding room type. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, RoomTypeRequest request)
        {
            try
            {
                var updatedRoomType = await roomTypeService.UpdateRoomTypeAsync(id, request);
                return Ok(ApiResponseFactory.Create(updatedRoomType, "Room type updated successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Room type with ID: {Id} not found for update.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while updating room type with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                var deletedRoomType = await roomTypeService.DeleteRoomTypeAsync(id);
                return Ok(ApiResponseFactory.Create(deletedRoomType, "Room type deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Room type with ID: {Id} not found for deletion.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while deleting room type with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }
        [HttpGet]
        [Route("{roomTypeId:int}/rooms")]
        public async Task<ActionResult<IEnumerable<RoomResponse>>> GetRoomsByRoomTypeId(int roomTypeId)
        {
            try
            {
                var rooms = await roomTypeService.GetRoomsByRoomTypeIdAsync(roomTypeId);
                return Ok(ApiResponseFactory.Create(rooms, "Rooms fetched successfully for the given RoomTypeId", 200, true));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching rooms for RoomTypeId: {RoomTypeId}", roomTypeId);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }
    }
}
