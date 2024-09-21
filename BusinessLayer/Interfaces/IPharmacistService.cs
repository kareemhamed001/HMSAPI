using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IPharmacistService
    {
        Task<IEnumerable<PharmacistResponse>> GetAllPharmacistsAsync();
        Task<PharmacistResponse> GetPharmacistByIdAsync(int id);
        Task<PharmacistResponse> CreatePharmacistAsync(PharmacistRequest pharmacistRequest);
        Task<PharmacistResponse> UpdatePharmacistAsync(int id, PharmacistRequest pharmacistRequest);
        Task<Pharmacist> DeletePharmacistAsync(int id);
    }
}
