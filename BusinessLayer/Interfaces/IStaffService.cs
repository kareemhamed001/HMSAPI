using SharedClasses.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IStaffService
    {
        Task<IEnumerable<StaffResponse>> GetAllStaffAsync();
        Task<StaffResponse> GetStaffByIdAsync(int id);
        Task<StaffResponse> CreateStaffAsync(StaffRequest staffRequest);
        Task<StaffResponse> UpdateStaffAsync(int id, StaffRequest staffRequest);
        Task<Staff> DeleteStaffAsync(int id);
    }
}
