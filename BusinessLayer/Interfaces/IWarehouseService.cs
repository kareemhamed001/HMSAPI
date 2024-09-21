using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace BusinessLayer.Interfaces
{
    public interface IWarehouseService
    {
        Task<IEnumerable<WarehouseResponse>> GetAllWarehousesAsync();
        Task<WarehouseResponse> GetWarehouseByIdAsync(int id);
        Task<WarehouseResponse> CreateWarehouseAsync(WarehouseRequest warehouseRequest);
        Task<WarehouseResponse> UpdateWarehouseAsync(int id, WarehouseRequest warehouseRequest);
        Task<Warehouse> DeleteWarehouseAsync(int id);
    }
}
