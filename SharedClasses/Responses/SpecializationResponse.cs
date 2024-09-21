using DataAccessLayer.Enums;

namespace SharedClasses.Responses
{
    public class SpecializationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public SpecializationTypesEnum Type { get; set; }
        public List<int> DoctorIds { get; set; } = new List<int>();
    }
}
