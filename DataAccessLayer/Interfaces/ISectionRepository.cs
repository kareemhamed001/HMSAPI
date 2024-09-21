using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace DataAccessLayer.Interfaces
{
    public interface ISectionRepository
    {
        Task<IEnumerable<SectionResponse>> GetAllSectionsAsync();
        Task<SectionResponse?> GetSectionByIdAsync(int id);
        Task<SectionResponse> CreateSectionAsync(Section section);
        Task<SectionResponse> UpdateSectionAsync(int id, Section section);
        Task<Section> DeleteSectionAsync(int id);
    }
}

