using Prueba_Tecnica.DTOs.Inventory;
using Prueba_Tecnica.DTOs.Reports;

namespace Prueba_Tecnica.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<InventoryReportDTO> RegisterMovementAsync(MovementRequestDTO dto);
        Task<IEnumerable<InventoryReportDTO>> RegisterTransferAsync(TransferRequestDTO dto);
        Task<DashboardStatsDTO> GetDashboardStatsAsync();
    }
}
