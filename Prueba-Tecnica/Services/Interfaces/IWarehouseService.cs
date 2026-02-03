using Prueba_Tecnica.DTOs.Warehouses;

namespace Prueba_Tecnica.Services.Interfaces
{
    public interface IWarehouseService
    {
        Task<IEnumerable<WarehouseDTO>> GetAllAsync();
        Task<WarehouseDTO> GetByIdAsync(int id);
        Task<WarehouseDTO> CreateAsync(WarehouseDTO dto);
        Task<bool> UpdateAsync(int id, WarehouseDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}