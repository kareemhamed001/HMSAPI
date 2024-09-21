using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<PatientResponse>> GetAllPatientsAsync();
        Task<PatientResponse?> GetPatientById(int id);
        Task<PatientResponse> CreatePatientAsync(Patient patient);
        Task<PatientResponse> UpdatePatientAsync(int id, Patient patient);
        Task<Patient> DeletePatientAsync(Patient patient);
    }
}
