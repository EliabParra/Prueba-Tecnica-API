using Microsoft.AspNetCore.Mvc;
using Prueba_Tecnica.DTOs.Inventory;
using Prueba_Tecnica.DTOs.Reports;
using Prueba_Tecnica.Services.Interfaces;

namespace Prueba_Tecnica.Controllers
{
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
    }
}
