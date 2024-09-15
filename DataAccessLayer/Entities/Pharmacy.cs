using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Pharmacy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public List<PharmacyMedicine> pharmacyMedicines { get; set; } = new List<PharmacyMedicine>();
        public List<ParmacistPharmacy> ParmacistPharmaces { get; set; } = new List<ParmacistPharmacy>();
    }
}
