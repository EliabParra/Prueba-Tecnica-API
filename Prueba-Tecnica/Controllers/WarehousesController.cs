using Microsoft.AspNetCore.Mvc;
using Prueba_Tecnica.DTOs.Warehouses;
using Prueba_Tecnica.Services.Interfaces;

namespace Prueba_Tecnica.Controllers
{
    [ApiController]
    [Route("api/warehouses")]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarehouseDTO>>> GetAll()
        {
            return Ok(await _warehouseService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WarehouseDTO>> GetById(int id)
        {
            var result = await _warehouseService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<WarehouseDTO>> Create(WarehouseDTO dto)
        {
            var result = await _warehouseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, WarehouseDTO dto)
        {
            var result = await _warehouseService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _warehouseService.DeleteAsync(id);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                // Retornamos 400 Bad Request si intentan borrar un almacén con stock
                return BadRequest(ex.Message);
            }
        }
    }
}