namespace SharedClasses.Responses
{
    public class ClinicResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SectionId { get; set; }
        public List<int> ReservationIds { get; set; } = new List<int>();
    }
}
