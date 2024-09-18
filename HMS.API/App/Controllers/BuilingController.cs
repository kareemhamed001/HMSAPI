
using LMSApi.App.Requests;

using AutoMapper;
using SharedClasses.Exceptions;
using SharedClasses.Responses;

namespace LMSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuldingsController : ControllerBase
    {
        private IBuildingService buildingService;
        private readonly ILogger<BuldingsController> logger;
        private readonly IMapper mapper;

        public BuldingsController(IBuildingService buildingService, ILogger<BuldingsController> logger, IMapper mapper)
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
    }
}
