using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IPrescriptionRepository
    {
        Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync();
        Task<PrescriptionResponse?> GetPrescriptionByIdAsync(int id);
        Task<PrescriptionResponse> CreatePrescriptionAsync(Prescription prescription);
        Task<PrescriptionResponse> UpdatePrescriptionAsync(int id, Prescription prescription);
        Task<Prescription> DeletePrescriptionAsync(Prescription prescription);
    }
}
