using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public List<PharmacyMedicine> pharmacyMedicines { get; set; }= new List<PharmacyMedicine>();
    }
}
