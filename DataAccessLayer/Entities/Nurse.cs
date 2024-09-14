namespace DataAccessLayer.Entities
{
    public class Nurse
    {
        public int Id { get; set; }
        public string? Passport { get; set; }
        public int? Experience { get; set; }
        public string? Education { get; set; }
        public string? AdditionalInfo { get; set; }
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
