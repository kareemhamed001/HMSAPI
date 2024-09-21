using System.Collections.Generic;
using System.Threading.Tasks;
using SharedClasses.Responses;

namespace BusinessLayer.Interfaces
{
    public interface IMedicineService
    {
        Task<IEnumerable<MedicineResponse>> GetAllMedicinesAsync();
        Task<MedicineResponse> GetMedicineByIdAsync(int id);
        Task<MedicineResponse> CreateMedicineAsync(MedicineRequest medicineRequest);
        Task<MedicineResponse> UpdateMedicineAsync(int id, MedicineRequest medicineRequest);
        Task<Medicine> DeleteMedicineAsync(int id);
    }
}
