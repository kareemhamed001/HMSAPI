using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace DataAccessLayer.Interfaces
{
    public interface IWarehouseRepository
    {
        Task<IEnumerable<WarehouseResponse>> GetAllWarehousesAsync();
        Task<WarehouseResponse?> GetWarehouseByIdAsync(int id);
        Task<WarehouseResponse> CreateWarehouseAsync(Warehouse warehouse);
        Task<WarehouseResponse> UpdateWarehouseAsync(int id, Warehouse warehouse);
        Task<Warehouse> DeleteWarehouseAsync(int id);
    }
}