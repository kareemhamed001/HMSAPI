using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace BusinessLayer.Interfaces
{
    public interface ISpecializationService
    {
        Task<IEnumerable<SpecializationResponse>> GetAllSpecializationsAsync();
        Task<SpecializationResponse> GetSpecializationByIdAsync(int id);
        Task<SpecializationResponse> CreateSpecializationAsync(SpecializationRequest specializationRequest);
        Task<SpecializationResponse> UpdateSpecializationAsync(int id, SpecializationRequest specializationRequest);
        Task<Specialization> DeleteSpecializationAsync(int id);
    }
}
