namespace BusinessLayer.Requests
{
    public class NurseRequest
    {
        public string? Passport { get; set; }
        public int? Experience { get; set; }
        public string? Education { get; set; }
        public string? AdditionalInfo { get; set; }
        public int SpecializationId { get; set; }
        public int UserId { get; set; }
    }
}
