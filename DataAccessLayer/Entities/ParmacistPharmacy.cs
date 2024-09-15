using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class ParmacistPharmacy
    {
        public int PharmasistId { get; set; }
        public Pharmacist Pharmacist { get; set; }
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }

    }
}
