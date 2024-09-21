using DataAccessLayer.Enums;

namespace BusinessLayer.Requests
{
    public class SpecializationRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public SpecializationTypesEnum Type { get; set; }
    }
}
    