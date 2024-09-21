using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IPharmacistRepository
    {
        Task<IEnumerable<PharmacistResponse>> GetAllPharmacistsAsync();
        Task<PharmacistResponse?> GetPharmacistByIdAsync(int id);
        Task<PharmacistResponse> CreatePharmacistAsync(Pharmacist pharmacist);
        Task<PharmacistResponse> UpdatePharmacistAsync(int id, Pharmacist pharmacist);
        Task<Pharmacist> DeletePharmacistAsync(Pharmacist pharmacist);
    }
}
