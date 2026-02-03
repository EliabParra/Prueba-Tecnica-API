using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prueba_Tecnica.DTOs.Products;
using Prueba_Tecnica.Services.Interfaces;

namespace Prueba_Tecnica.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products?search=lapiz&categoryId=2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductListDTO>>> GetAll([FromQuery] string? search, [FromQuery] int? categoryId)
        {
            var products = await _productService.GetFilteredAsync(search, categoryId);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Create(CreateProductDTO dto)
        {
            var createdProduct = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDTO dto)
        {
            var result = await _productService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.SoftDeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}