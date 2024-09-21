using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace BusinessLayer.Interfaces
{
    public interface INurseService
    {
        Task<IEnumerable<NurseResponse>> GetAllNursesAsync();
        Task<NurseResponse> GetNurseByIdAsync(int id);
        Task<NurseResponse> CreateNurseAsync(NurseRequest nurseRequest);
        Task<NurseResponse> UpdateNurseAsync(int id, NurseRequest nurseRequest);
        Task<Nurse> DeleteNurseAsync(int id);
    }
}
