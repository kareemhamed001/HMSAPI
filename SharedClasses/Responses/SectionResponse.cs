namespace SharedClasses.Responses
{
    public class SectionResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> ClinicIds { get; set; } = new List<int>();
        public List<int> WarehouseIds { get; set; } = new List<int>();
    }
}
