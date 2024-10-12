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
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ILogger<SupplierService> _logger;
        private readonly IMapper _mapper;

        public SupplierService(ISupplierRepository supplierRepository, ILogger<SupplierService> logger, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SupplierResponse> CreateSupplierAsync(SupplierRequest supplierRequest)
        {
            try
            {
                Supplier supplier = _mapper.Map<Supplier>(supplierRequest);
                return await _supplierRepository.CreateSupplierAsync(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating supplier with Name: {Name}", supplierRequest.Name);
                throw;
            }
        }

        public async Task<Supplier> DeleteSupplierAsync(int id)
        {
            try
            {
                var supplierResponse = await _supplierRepository.GetSupplierByIdAsync(id);
                if (supplierResponse == null)
                {
                    _logger.LogWarning("Supplier with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Supplier not found.");
                }

                return await _supplierRepository.DeleteSupplierAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Supplier with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<SupplierResponse>> GetAllSuppliersAsync()
        {
            try
            {
                return await _supplierRepository.GetAllSuppliersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all suppliers.");
                throw;
            }
        }

        public async Task<SupplierResponse> GetSupplierByIdAsync(int id)
        {
            try
            {
                var supplier = await _supplierRepository.GetSupplierByIdAsync(id);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID: {Id} not found.", id);
                    throw new NotFoundException("Supplier not found.");
                }
                return supplier;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching supplier with ID: {Id}", id);
                throw;
            }
        }

        public async Task<SupplierResponse> UpdateSupplierAsync(int id, SupplierRequest supplierRequest)
        {
            try
            {
                var existingSupplier = await _supplierRepository.GetSupplierByIdAsync(id);
                if (existingSupplier == null)
                {
                    _logger.LogWarning("Supplier with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Supplier not found.");
                }

                Supplier supplier = _mapper.Map<Supplier>(existingSupplier);
                _mapper.Map(supplierRequest, supplier);

                return await _supplierRepository.UpdateSupplierAsync(id, supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Supplier with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<MedicineResponse>> GetMedicinesBySupplierIdAsync(int supplierId)
        {
            try
            {
                return await _supplierRepository.GetMedicinesBySupplierIdAsync(supplierId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching medicines for SupplierId: {SupplierId}", supplierId);
                throw;
            }
        }
    }
}
