using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class ClinicDoctor
    {
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
