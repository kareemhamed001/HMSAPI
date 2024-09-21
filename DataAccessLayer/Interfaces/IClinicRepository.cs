using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace DataAccessLayer.Interfaces
{
    public interface IClinicRepository
    {
        Task<IEnumerable<ClinicResponse>> GetAllClinicsAsync();
        Task<ClinicResponse?> GetClinicByIdAsync(int id);
        Task<ClinicResponse> CreateClinicAsync(Clinic clinic);
        Task<ClinicResponse> UpdateClinicAsync(int id, Clinic clinic);
        Task<Clinic> DeleteClinicAsync(int id);
    }
}
