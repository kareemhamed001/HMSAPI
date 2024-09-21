using System.Collections.Generic;

namespace SharedClasses.Responses
{
    public class PatientResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<ReservationResponse> Reservations { get; set; } = new List<ReservationResponse>();
        public List<PrescriptionResponse> Prescriptions { get; set; } = new List<PrescriptionResponse>();
    }
}
