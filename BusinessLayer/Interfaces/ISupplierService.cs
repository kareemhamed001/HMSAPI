using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace BusinessLayer.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierResponse>> GetAllSuppliersAsync();
        Task<SupplierResponse> GetSupplierByIdAsync(int id);
        Task<SupplierResponse> CreateSupplierAsync(SupplierRequest supplierRequest);
        Task<SupplierResponse> UpdateSupplierAsync(int id, SupplierRequest supplierRequest);
        Task<Supplier> DeleteSupplierAsync(int id);
        Task<IEnumerable<MedicineResponse>> GetMedicinesBySupplierIdAsync(int supplierId);
    }
}
