using SharedClasses.Responses;


namespace DataAccessLayer.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly AppDbContext _context;
        public BuildingRepository(AppDbContext context)
        {
            _context = context;

        }
        public async Task<BuildingResponse> CreateBuildingAsync(Building building)
        {
            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();
            BuildingResponse response = new BuildingResponse
            {
                Id = building.Id,
                Name = building.Name,
                Description = building.Description,
                address = building.address
            };
            return response;
        }

        public async Task<Building> DeleteBuildingAsync(Building building)
        {
            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();
            return building;
        }

        public async Task<IEnumerable<BuildingResponse>> GetAllBuildingsAsync()
        {
            var buildings = await _context.Buildings
                .Select(br => new BuildingResponse
                {
                    Id = br.Id,
                    Name = br.Name,
                    Description = br.Description,
                    address = br.address
                })
                .AsNoTracking()
                .ToListAsync();
            return buildings;
        }

        public async Task<BuildingResponse?> GetBuildingById(int id)
        {
            return await _context.Buildings.AsNoTracking()
                .Select(br => new BuildingResponse
                {
                    Id = br.Id,
                    Name = br.Name,
                    Description = br.Description,
                    address = br.address
                })
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BuildingResponse> UpdateBuildingAsync(Building building)
        {
            _context.Buildings.Update(building);
            await _context.SaveChangesAsync();
            BuildingResponse response = new BuildingResponse
            {
                Id = building.Id,
                Name = building.Name,
                Description = building.Description,
                address = building.address
            };
            return response;
        }


        // Additional methods
        public async Task<IEnumerable<Floor>> GetFloorsByBuildingId(int buildingId)
        {
            var building = await _context.Buildings.Include(b => b.Floors).FirstOrDefaultAsync(b => b.Id == buildingId);
            var floors = building.Floors;
            return floors;
        }

        public async Task<Building> BuildingExist(int id)
        {
            var building = await _context.Buildings
             .AsNoTracking()
             .FirstOrDefaultAsync(b => b.Id == id);
            return building;
        }
    }
}
