using SharedClasses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IPharmacyRepository
    {
        Task<IEnumerable<PharmacyResponse>> GetAllPharmacyAsync();
        Task<PharmacyResponse?> GetPharmacyById(int id);
        Task<PharmacyResponse> CreatePharmacyAsync(Pharmacy pharmacy);
        Task<PharmacyResponse> UpdatePharmacyAsync(int id, Pharmacy pharmacy);
        Task<Pharmacy> DeletePharmacyAsync(Pharmacy pharmacy);
    }
}
