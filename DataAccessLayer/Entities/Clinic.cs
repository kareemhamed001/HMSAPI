using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SectionId { get; set; }
        public Section Section { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public List<ClinicDoctor> ClinicDoctors { get; set; } = new List<ClinicDoctor>();
    }
}
