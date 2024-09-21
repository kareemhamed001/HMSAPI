using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace DataAccessLayer.Interfaces
{
    public interface INurseRepository
    {
        Task<IEnumerable<NurseResponse>> GetAllNursesAsync();
        Task<NurseResponse?> GetNurseByIdAsync(int id);
        Task<NurseResponse> CreateNurseAsync(Nurse nurse);
        Task<NurseResponse> UpdateNurseAsync(int id, Nurse nurse);
        Task<Nurse> DeleteNurseAsync(int id);
    }
}
