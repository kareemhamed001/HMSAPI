using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IPharmacyService
    {
        Task<IEnumerable<PharmacyResponse>> GetAllPharmaciesAsync();
        Task<PharmacyResponse> GetPharmacyByIdAsync(int id);
        Task<PharmacyResponse> CreatePharmacyAsync(PharmacyRequest pharmacyRequest);
        Task<PharmacyResponse> UpdatePharmacyAsync(int id, PharmacyRequest pharmacyRequest);
        Task<Pharmacy> DeletePharmacyAsync(int id);
        Task<IEnumerable<MedicineResponse>> GetMedicinesByPharmacyIdAsync(int id);
    }
}
