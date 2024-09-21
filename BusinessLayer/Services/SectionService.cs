using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Requests;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Logging;
using SharedClasses.Exceptions;
using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly ILogger<SectionService> _logger;
        private readonly IMapper _mapper;

        public SectionService(ISectionRepository sectionRepository, ILogger<SectionService> logger, IMapper mapper)
        {
            _sectionRepository = sectionRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SectionResponse> CreateSectionAsync(SectionRequest sectionRequest)
        {
            try
            {
                Section section = _mapper.Map<Section>(sectionRequest);
                return await _sectionRepository.CreateSectionAsync(section);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating section with Name: {Name}", sectionRequest.Name);
                throw;
            }
        }

        public async Task<Section> DeleteSectionAsync(int id)
        {
            try
            {
                var sectionResponse = await _sectionRepository.GetSectionByIdAsync(id);
                if (sectionResponse == null)
                {
                    _logger.LogWarning("Section with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Section not found.");
                }

                return await _sectionRepository.DeleteSectionAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Section with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<SectionResponse>> GetAllSectionsAsync()
        {
            try
            {
                return await _sectionRepository.GetAllSectionsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all sections.");
                throw;
            }
        }

        public async Task<SectionResponse> GetSectionByIdAsync(int id)
        {
            try
            {
                var section = await _sectionRepository.GetSectionByIdAsync(id);
                if (section == null)
                {
                    _logger.LogWarning("Section with ID: {Id} not found.", id);
                    throw new NotFoundException("Section not found.");
                }
                return section;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching section with ID: {Id}", id);
                throw;
            }
        }

        public async Task<SectionResponse> UpdateSectionAsync(int id, SectionRequest sectionRequest)
        {
            try
            {
                var existingSection = await _sectionRepository.GetSectionByIdAsync(id);
                if (existingSection == null)
                {
                    _logger.LogWarning("Section with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Section not found.");
                }

                Section section = _mapper.Map<Section>(existingSection);
                _mapper.Map(sectionRequest, section);

                return await _sectionRepository.UpdateSectionAsync(id, section);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Section with ID: {Id}", id);
                throw;
            }
        }
    }
}
