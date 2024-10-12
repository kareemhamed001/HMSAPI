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
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService roomService;
        private readonly ILogger<RoomsController> logger;
        private readonly IMapper mapper;

        public RoomsController(IRoomService roomService, ILogger<RoomsController> logger, IMapper mapper)
        {
            this.roomService = roomService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllRooms()
        {
            try
            {
                var rooms = await roomService.GetAllRoomAsync();
                return Ok(ApiResponseFactory.Create(rooms, "Rooms fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while fetching rooms. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> GetRoomById(int id)
        {
            try
            {
                var room = await roomService.GetRoomByIdAsync(id);
                return Ok(ApiResponseFactory.Create(room, "Room fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Room with ID: {Id} not found.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while fetching room with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> Add(RoomRequest request)
        {
            try
            {
                var addedRoom = await roomService.CreateRoomAsync(request);
                return Ok(ApiResponseFactory.Create(addedRoom, "Room added successfully", 200, true));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while adding room. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, RoomRequest request)
        {
            try
            {
                var updatedRoom = await roomService.UpdateRoomAsync(id, request);
                return Ok(ApiResponseFactory.Create(updatedRoom, "Room updated successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Room with ID: {Id} not found for update.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while updating room with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                var deletedRoom = await roomService.DeleteRoomAsync(id);
                return Ok(ApiResponseFactory.Create(deletedRoom, "Room deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Room with ID: {Id} not found for deletion.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while deleting room with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{id:int}/room-type")]
        public async Task<ActionResult<IApiResponse>> GetRoomTypeByRoomId(int id)
        {
            try
            {
                var roomType = await roomService.GetRoomTypeByRoomIdAsync(id);
                return Ok(ApiResponseFactory.Create(roomType, "Room type fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "RoomType for Room with ID: {Id} not found.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while fetching RoomType for Room with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }
    }
}
