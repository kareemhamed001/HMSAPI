using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorResponse>> GetAllDoctorsAsync();
        Task<DoctorResponse> GetDoctorByIdAsync(int id);
        Task<DoctorResponse> CreateDoctorAsync(DoctorRequest doctorRequest);
        Task<DoctorResponse> UpdateDoctorAsync(int id, DoctorRequest doctorRequest);
        Task<Doctor> DeleteDoctorAsync(int id);
    }
}
