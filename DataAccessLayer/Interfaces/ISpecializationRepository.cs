using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace DataAccessLayer.Interfaces
{
    public interface ISpecializationRepository
    {
        Task<IEnumerable<SpecializationResponse>> GetAllSpecializationsAsync();
        Task<SpecializationResponse?> GetSpecializationByIdAsync(int id);
        Task<SpecializationResponse> CreateSpecializationAsync(Specialization specialization);
        Task<SpecializationResponse> UpdateSpecializationAsync(int id, Specialization specialization);
        Task<Specialization> DeleteSpecializationAsync(int id);
    }
}
