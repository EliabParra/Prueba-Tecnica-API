using Prueba_Tecnica.DTOs.Reports;

namespace Prueba_Tecnica.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<InventoryReportDTO>> GetInventoryReportAsync(int? productId, int? warehouseId);
        Task<byte[]> GetInventoryReportExcelAsync(int? productId, int? warehouseId);
    }
}
