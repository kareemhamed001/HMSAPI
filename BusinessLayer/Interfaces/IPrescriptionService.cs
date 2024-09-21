using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync();
        Task<PrescriptionResponse> GetPrescriptionByIdAsync(int id);
        Task<PrescriptionResponse> CreatePrescriptionAsync(PrescriptionRequest prescriptionRequest);
        Task<PrescriptionResponse> UpdatePrescriptionAsync(int id, PrescriptionRequest prescriptionRequest);
        Task<Prescription> DeletePrescriptionAsync(int id);
    }
}
