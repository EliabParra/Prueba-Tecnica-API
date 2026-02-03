using Prueba_Tecnica.DTOs.Products;

namespace Prueba_Tecnica.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductListDTO>> GetFilteredAsync(string? search, int? categoryId);

        Task<ProductDTO> GetByIdAsync(int id);
        Task<ProductDTO> CreateAsync(CreateProductDTO dto);
        Task<bool> UpdateAsync(int id, UpdateProductDTO dto);

        Task<bool> SoftDeleteAsync(int id);
    }
}