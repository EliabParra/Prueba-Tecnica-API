using Prueba_Tecnica.DTOs.Products;

namespace Prueba_Tecnica.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductListDTO> GetFilteredAsync(string search, int? categoryId);
        ProductDTO GetByIdAsync()
    }
}
