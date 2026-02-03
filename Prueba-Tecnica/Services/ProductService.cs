using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prueba_Tecnica.Data;
using Prueba_Tecnica.DTOs.Products;
using Prueba_Tecnica.Models;
using Prueba_Tecnica.Services.Interfaces;

namespace Prueba_Tecnica.Services
{
    public class ProductService : IProductService
    {
        private readonly InventoryDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(InventoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductListDTO>> GetFilteredAsync(string? search, int? categoryId)
        {
            // filtrar no eliminados
            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            var products = await query.ToListAsync();

            return _mapper.Map<IEnumerable<ProductListDTO>>(products);
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            // buscar no eliminado
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> CreateAsync(CreateProductDTO dto)
        {
            var product = _mapper.Map<Product>(dto);

            product.IsDeleted = false;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            await _context.Entry(product).Reference(p => p.Category).LoadAsync();

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDTO dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null || product.IsDeleted) return false;

            _mapper.Map(dto, product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || product.IsDeleted) return false;

            product.IsDeleted = true;


            await _context.SaveChangesAsync();
            return true;
        }
    }
}