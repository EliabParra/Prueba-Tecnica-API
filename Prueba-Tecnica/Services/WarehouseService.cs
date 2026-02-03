using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prueba_Tecnica.Data;
using Prueba_Tecnica.DTOs.Warehouses;
using Prueba_Tecnica.Models;
using Prueba_Tecnica.Services.Interfaces;

namespace Prueba_Tecnica.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly InventoryDbContext _context;
        private readonly IMapper _mapper;

        public WarehouseService(InventoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WarehouseDTO>> GetAllAsync()
        {
            var warehouses = await _context.Warehouses.ToListAsync();
            return _mapper.Map<IEnumerable<WarehouseDTO>>(warehouses);
        }

        public async Task<WarehouseDTO> GetByIdAsync(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            return _mapper.Map<WarehouseDTO>(warehouse);
        }

        public async Task<WarehouseDTO> CreateAsync(WarehouseDTO dto)
        {
            var warehouse = _mapper.Map<Warehouse>(dto);

            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();

            return _mapper.Map<WarehouseDTO>(warehouse);
        }

        public async Task<bool> UpdateAsync(int id, WarehouseDTO dto)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null) return false;

            _mapper.Map(dto, warehouse);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null) return false;

            // verificar stock
            var hasStock = await _context.ProductWarehouses.AnyAsync(pw => pw.WarehouseId == id && pw.CurrentStock > 0);
            if (hasStock)
            {
                throw new Exception("No se puede eliminar un almacen con stock activo.");
            }

            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}