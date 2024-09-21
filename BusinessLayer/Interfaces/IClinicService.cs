using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace BusinessLayer.Interfaces
{
    public interface IClinicService
    {
        Task<IEnumerable<ClinicResponse>> GetAllClinicsAsync();
        Task<ClinicResponse> GetClinicByIdAsync(int id);
        Task<ClinicResponse> CreateClinicAsync(ClinicRequest clinicRequest);
        Task<ClinicResponse> UpdateClinicAsync(int id, ClinicRequest clinicRequest);
        Task<Clinic> DeleteClinicAsync(int id);
    }
}
