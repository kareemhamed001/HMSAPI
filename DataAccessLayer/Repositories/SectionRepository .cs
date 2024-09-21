using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using SharedClasses.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly AppDbContext _context;

        public SectionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SectionResponse> CreateSectionAsync(Section section)
        {
            _context.Sections.Add(section);
            await _context.SaveChangesAsync();
            return new SectionResponse
            {
                Id = section.Id,
                Name = section.Name,
                Description = section.Description,
                ClinicIds = section.Clinics.Select(c => c.Id).ToList(),
                WarehouseIds = section.Warehouses.Select(w => w.Id).ToList()
            };
        }

        public async Task<Section> DeleteSectionAsync(int id)
        {
            var section = await _context.Sections
                .Include(s => s.Clinics)
                .Include(s => s.Warehouses)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (section != null)
            {
                _context.Sections.Remove(section);
                await _context.SaveChangesAsync();
                return section;
            }
            return null;
        }

        public async Task<IEnumerable<SectionResponse>> GetAllSectionsAsync()
        {
            return await _context.Sections
                .Select(s => new SectionResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ClinicIds = s.Clinics.Select(c => c.Id).ToList(),
                    WarehouseIds = s.Warehouses.Select(w => w.Id).ToList()
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SectionResponse?> GetSectionByIdAsync(int id)
        {
            return await _context.Sections
                .Where(s => s.Id == id)
                .Select(s => new SectionResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ClinicIds = s.Clinics.Select(c => c.Id).ToList(),
                    WarehouseIds = s.Warehouses.Select(w => w.Id).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<SectionResponse> UpdateSectionAsync(int id, Section section)
        {
            _context.Sections.Update(section);
            await _context.SaveChangesAsync();
            return new SectionResponse
            {
                Id = section.Id,
                Name = section.Name,
                Description = section.Description,
                ClinicIds = section.Clinics.Select(c => c.Id).ToList(),
                WarehouseIds = section.Warehouses.Select(w => w.Id).ToList()
            };
        }
    }
}
