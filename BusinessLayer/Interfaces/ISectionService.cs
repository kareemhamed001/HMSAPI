using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace BusinessLayer.Interfaces
{
    public interface ISectionService
    {
        Task<IEnumerable<SectionResponse>> GetAllSectionsAsync();
        Task<SectionResponse> GetSectionByIdAsync(int id);
        Task<SectionResponse> CreateSectionAsync(SectionRequest sectionRequest);
        Task<SectionResponse> UpdateSectionAsync(int id, SectionRequest sectionRequest);
        Task<Section> DeleteSectionAsync(int id);
    }
}
