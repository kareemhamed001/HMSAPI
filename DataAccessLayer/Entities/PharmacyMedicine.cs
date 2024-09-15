using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class PharmacyMedicine
    {
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
    }
}
