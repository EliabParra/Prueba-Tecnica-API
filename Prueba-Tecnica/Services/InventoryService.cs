using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prueba_Tecnica.Data;
using Prueba_Tecnica.DTOs.Inventory;
using Prueba_Tecnica.DTOs.Reports;
using Prueba_Tecnica.Models;
using Prueba_Tecnica.Services.Interfaces;

namespace Prueba_Tecnica.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryDbContext _context;
        private readonly IMapper _mapper;

        public InventoryService(InventoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InventoryReportDTO> RegisterMovementAsync(MovementRequestDTO dto)
        {
            if (dto.Quantity <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a 0");
            }

            var movementType = dto.MovementType?.Trim().ToUpperInvariant();
            if (movementType is not ("IN" or "OUT"))
            {
                throw new ArgumentException("El tipo de movimiento solo puede ser IN o OUT");
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == dto.ProductId && !p.IsDeleted);

            if (product == null)
            {
                throw new InvalidOperationException("Producto no encontrado");
            }

            var warehouse = await _context.Warehouses.FindAsync(dto.WarehouseId);
            if (warehouse == null)
            {
                throw new InvalidOperationException("Almacen no encontrado");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            var productWarehouse = await _context.ProductWarehouses
                .FirstOrDefaultAsync(pw => pw.ProductId == dto.ProductId && pw.WarehouseId == dto.WarehouseId);

            if (productWarehouse == null)
            {
                productWarehouse = new ProductWarehouse
                {
                    ProductId = dto.ProductId,
                    WarehouseId = dto.WarehouseId,
                    CurrentStock = 0
                };
                _context.ProductWarehouses.Add(productWarehouse);
            }

            if (movementType == "OUT" && productWarehouse.CurrentStock < dto.Quantity)
            {
                throw new InvalidOperationException("Stock insuficiente para realizar movimiento");
            }

            productWarehouse.CurrentStock = movementType == "IN"
                ? productWarehouse.CurrentStock + dto.Quantity
                : productWarehouse.CurrentStock - dto.Quantity;

            var movement = _mapper.Map<InventoryMovements>(dto);
            movement.MovementType = movementType;

            _context.InventoryMovements.Add(movement);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            await _context.Entry(movement).Reference(m => m.Product).LoadAsync();
            await _context.Entry(movement).Reference(m => m.Warehouse).LoadAsync();

            return _mapper.Map<InventoryReportDTO>(movement);
        }
    }
}
