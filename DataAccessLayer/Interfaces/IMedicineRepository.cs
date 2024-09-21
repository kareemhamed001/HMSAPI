using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace DataAccessLayer.Interfaces
{
    public interface IMedicineRepository
    {
        Task<IEnumerable<MedicineResponse>> GetAllMedicinesAsync();
        Task<MedicineResponse?> GetMedicineByIdAsync(int id);
        Task<MedicineResponse> CreateMedicineAsync(Medicine medicine);
        Task<MedicineResponse> UpdateMedicineAsync(int id, Medicine medicine);
        Task<Medicine> DeleteMedicineAsync(int id);
    }
}
