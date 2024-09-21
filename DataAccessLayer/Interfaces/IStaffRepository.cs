using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IStaffRepository
    {
        Task<IEnumerable<StaffResponse>> GetAllStaffAsync();
        Task<StaffResponse?> GetStaffByIdAsync(int id);
        Task<StaffResponse> CreateStaffAsync(Staff staff);
        Task<StaffResponse> UpdateStaffAsync(int id, Staff staff);
        Task<Staff> DeleteStaffAsync(Staff staff);
    }
}
