namespace SharedClasses.Responses
{
    public class ReservationResponse
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int ClinicId { get; set; }
    }
}
