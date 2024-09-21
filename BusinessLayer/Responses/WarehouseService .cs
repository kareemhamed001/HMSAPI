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
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ILogger<WarehouseService> _logger;
        private readonly IMapper _mapper;

        public WarehouseService(IWarehouseRepository warehouseRepository, ILogger<WarehouseService> logger, IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<WarehouseResponse> CreateWarehouseAsync(WarehouseRequest warehouseRequest)
        {
            try
            {
                Warehouse warehouse = _mapper.Map<Warehouse>(warehouseRequest);
                return await _warehouseRepository.CreateWarehouseAsync(warehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating warehouse with Name: {Name}", warehouseRequest.Name);
                throw;
            }
        }

        public async Task<Warehouse> DeleteWarehouseAsync(int id)
        {
            try
            {
                var warehouseResponse = await _warehouseRepository.GetWarehouseByIdAsync(id);
                if (warehouseResponse == null)
                {
                    _logger.LogWarning("Warehouse with ID: {Id} not found for deletion.", id);
                    throw new NotFoundException("Warehouse not found.");
                }

                return await _warehouseRepository.DeleteWarehouseAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Warehouse with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<WarehouseResponse>> GetAllWarehousesAsync()
        {
            try
            {
                return await _warehouseRepository.GetAllWarehousesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all warehouses.");
                throw;
            }
        }

        public async Task<WarehouseResponse> GetWarehouseByIdAsync(int id)
        {
            try
            {
                var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(id);
                if (warehouse == null)
                {
                    _logger.LogWarning("Warehouse with ID: {Id} not found.", id);
                    throw new NotFoundException("Warehouse not found.");
                }
                return warehouse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching warehouse with ID: {Id}", id);
                throw;
            }
        }

        public async Task<WarehouseResponse> UpdateWarehouseAsync(int id, WarehouseRequest warehouseRequest)
        {
            try
            {
                var existingWarehouse = await _warehouseRepository.GetWarehouseByIdAsync(id);
                if (existingWarehouse == null)
                {
                    _logger.LogWarning("Warehouse with ID: {Id} not found for update.", id);
                    throw new NotFoundException("Warehouse not found.");
                }

                Warehouse warehouse = _mapper.Map<Warehouse>(existingWarehouse);
                _mapper.Map(warehouseRequest, warehouse);

                return await _warehouseRepository.UpdateWarehouseAsync(id, warehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Warehouse with ID: {Id}", id);
                throw;
            }
        }
    }
}
