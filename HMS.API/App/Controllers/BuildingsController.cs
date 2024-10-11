
using LMSApi.App.Requests;

using AutoMapper;
using SharedClasses.Exceptions;
using SharedClasses.Responses;

namespace LMSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingsController : ControllerBase
    {
        private IBuildingService buildingService;
        private readonly ILogger<BuildingsController> logger;
        private readonly IMapper mapper;

        public BuildingsController(IBuildingService buildingService, ILogger<BuildingsController> logger, IMapper mapper)
        {
            this.buildingService = buildingService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> GetAllBuildings()
        {
            try
            {
                var buildings = await buildingService.GetAllBuildingsAsync();
                var buildingResponses = mapper.Map<List<BuildingResponse>>(buildings);
                return Ok(ApiResponseFactory.Create(buildingResponses, "Buildings fetched successfully", 200, true));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while fetching buildings. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> GetBuildingById(int id)
        {
            try
            {
                var building = await buildingService.GetBuildingByIdAsync(id);
                var buildingResponse = mapper.Map<BuildingResponse>(building);
                return Ok(ApiResponseFactory.Create(buildingResponse, "Building fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Building with ID: {Id} not found.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while fetching building with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }


        [HttpPost]
        [Route("")]
        public async Task<ActionResult<IApiResponse>> Add(BuildingRequest request)
        {
            try
            {
                var addedBuilding = await buildingService.CreateBuildingAsync(request);
                var buildingResponse = mapper.Map<BuildingResponse>(addedBuilding);
                return Ok(ApiResponseFactory.Create(buildingResponse, "Building added successfully", 200, true));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while adding building. Log message: {logMessage}", ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Update(int id, BuildingRequest request)
        {
            try
            {
                var updatedBuilding = await buildingService.UpdateBuildingAsync(id, request);
                var buildingResponse = mapper.Map<BuildingResponse>(updatedBuilding);
                return Ok(ApiResponseFactory.Create(buildingResponse, "Building updated successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Building with ID: {Id} not found for update.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while updating building with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<IApiResponse>> Delete(int id)
        {
            try
            {
                var deletedBuilding = await buildingService.DeleteBuildingAsync(id);
                return Ok(ApiResponseFactory.Create(deletedBuilding, "Building deleted successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Building with ID: {Id} not found for deletion.", id);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while deleting building with ID: {Id}. Log message: {logMessage}", id, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }

        [HttpGet]
        [Route("{buildingId:int}/floors")]
        public async Task<ActionResult<IApiResponse>> GetFloorsByBuildingId(int buildingId)
        {
            try
            {
                var floors = await buildingService.GetFloorsByBuildingIdAsync(buildingId);
                return Ok(ApiResponseFactory.Create(floors, "Floors fetched successfully", 200, true));
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Floors not found for building with ID: {BuildingId}.", buildingId);
                return NotFound(ApiResponseFactory.Create(ex.Message, 404, false));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while fetching floors for building with ID: {BuildingId}. Log message: {logMessage}", buildingId, ex);
                return StatusCode(500, ApiResponseFactory.Create(ex.Message, 500, false));
            }
        }


    }
}
