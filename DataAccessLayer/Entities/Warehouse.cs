namespace DataAccessLayer.Entities
{
    public class Warehouse
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int SectionId { get; set; }
        public Section Section { get; set; }
    }
}
