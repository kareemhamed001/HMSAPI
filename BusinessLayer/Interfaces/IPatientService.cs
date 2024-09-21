using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientResponse>> GetAllPatientsAsync();
        Task<PatientResponse> GetPatientByIdAsync(int id);
        Task<PatientResponse> CreatePatientAsync(PatientRequest patientRequest);
        Task<PatientResponse> UpdatePatientAsync(int id, PatientRequest patientRequest);
        Task<Patient> DeletePatientAsync(int id);
    }
}
