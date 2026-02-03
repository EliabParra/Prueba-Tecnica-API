using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prueba_Tecnica.DTOs.Reports;
using Prueba_Tecnica.Services.Interfaces;

namespace Prueba_Tecnica.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("inventory")]
        public async Task<ActionResult<IEnumerable<InventoryReportDTO>>> GetInventoryReport(
            [FromQuery] int? productId,
            [FromQuery] int? warehouseId)
        {
            var result = await _reportService.GetInventoryReportAsync(productId, warehouseId);
            return Ok(result);
        }

        [HttpGet("inventory/excel")]
        public async Task<IActionResult> ExportInventoryReportExcel(
            [FromQuery] int productId,
            [FromQuery] int? warehouseId)
        {
            if (productId <= 0)
            {
                return BadRequest("El id del producto es obligatorio");
            }

            var content = await _reportService.GetInventoryReportExcelAsync(productId, warehouseId);
            var fileName = $"reporte-inventario-{DateTime.Now:ddMMyyyyHHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
