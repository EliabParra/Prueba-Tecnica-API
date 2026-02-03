using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prueba_Tecnica.DTOs.Inventory;
using Prueba_Tecnica.DTOs.Reports;
using Prueba_Tecnica.Services.Interfaces;

namespace Prueba_Tecnica.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost("movement")]
        public async Task<ActionResult<InventoryReportDTO>> RegisterMovement(MovementRequestDTO dto)
        {
            try
            {
                var result = await _inventoryService.RegisterMovementAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<IEnumerable<InventoryReportDTO>>> RegisterTransfer(TransferRequestDTO dto)
        {
            try
            {
                var result = await _inventoryService.RegisterTransferAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardStatsDTO>> GetDashboardStats()
        {
            var result = await _inventoryService.GetDashboardStatsAsync();
            return Ok(result);
        }

        [HttpGet("stock")]
        public async Task<ActionResult<IEnumerable<InventoryStockDTO>>> GetStock(
            [FromQuery] int? productId,
            [FromQuery] int? warehouseId)
        {
            var result = await _inventoryService.GetStockAsync(productId, warehouseId);
            return Ok(result);
        }
    }
}
