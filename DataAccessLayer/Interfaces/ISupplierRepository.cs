using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace DataAccessLayer.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<SupplierResponse>> GetAllSuppliersAsync();
        Task<SupplierResponse?> GetSupplierByIdAsync(int id);
        Task<SupplierResponse> CreateSupplierAsync(Supplier supplier);
        Task<SupplierResponse> UpdateSupplierAsync(int id, Supplier supplier);
        Task<Supplier> DeleteSupplierAsync(int id);
        Task<IEnumerable<MedicineResponse>> GetMedicinesBySupplierIdAsync(int supplierId);
    }
}
