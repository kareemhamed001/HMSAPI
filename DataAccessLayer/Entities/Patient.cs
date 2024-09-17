using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<MedicalHistory> MedicalHistories { get; set; }=new List<MedicalHistory>();
        public List<Reservation> Reservations { get; set; }= new List<Reservation>();
        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
