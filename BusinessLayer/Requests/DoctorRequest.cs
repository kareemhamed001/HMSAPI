namespace BusinessLayer.Requests
{
    public class DoctorRequest
    {
        public string Description { get; set; }
        public string Education { get; set; }
        public int SpecializationId { get; set; }
        public int UserId { get; set; }
    }
}
