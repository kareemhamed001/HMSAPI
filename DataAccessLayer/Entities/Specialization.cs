using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public SpecializationTypesEnum Type { get; set; }
        public List<Doctor> Doctors { get; set; }

    }
}
