namespace DataAccessLayer.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
