namespace DataAccessLayer.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Education { get; set; }
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<MedicalHistory> MedicalHistories { get; set; }= new List<MedicalHistory>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public List<ClinicDoctor> ClinicDoctors { get; set; } = new List<ClinicDoctor>();
        public List<Prescription> Prescriptions { get; set;} = new List<Prescription>();
    }
}
