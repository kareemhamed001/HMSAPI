using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<DoctorResponse>> GetAllDoctorsAsync();
        Task<DoctorResponse?> GetDoctorByIdAsync(int id);
        Task<DoctorResponse> CreateDoctorAsync(Doctor doctor);
        Task<DoctorResponse> UpdateDoctorAsync(int id, Doctor doctor);
        Task<Doctor> DeleteDoctorAsync(Doctor doctor);
    }
}
