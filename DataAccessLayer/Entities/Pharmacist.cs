namespace DataAccessLayer.Entities
{
    public class Pharmacist
    {
        public int Id { get; set; }
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<ParmacistPharmacy> ParmacistPharmaces { get; set; }=new List<ParmacistPharmacy>();
    }
}
