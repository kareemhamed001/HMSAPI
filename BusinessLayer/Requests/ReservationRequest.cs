namespace BusinessLayer.Requests
{
    public class ReservationRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int ClinicId { get; set; }
    }
}
