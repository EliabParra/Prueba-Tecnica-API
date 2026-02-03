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

        public async Task<IEnumerable<InventoryReportDTO>> RegisterTransferAsync(TransferRequestDTO dto)
        {
            if (dto.Quantity <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a 0");
            }

            if (dto.SourceWarehouseId == dto.TargetWarehouseId)
            {
                throw new ArgumentException("Los almacenes de origen y destino deben ser diferentes");
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == dto.ProductId && !p.IsDeleted);

            if (product == null)
            {
                throw new InvalidOperationException("Producto no encontrado");
            }

            var sourceWarehouse = await _context.Warehouses.FindAsync(dto.SourceWarehouseId);
            if (sourceWarehouse == null)
            {
                throw new InvalidOperationException("Producto no encontrado");
            }

            var targetWarehouse = await _context.Warehouses.FindAsync(dto.TargetWarehouseId);
            if (targetWarehouse == null)
            {
                throw new InvalidOperationException("Almacen de destino no encontrado");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            var sourceStock = await _context.ProductWarehouses
                .FirstOrDefaultAsync(pw => pw.ProductId == dto.ProductId && pw.WarehouseId == dto.SourceWarehouseId);

            if (sourceStock == null || sourceStock.CurrentStock < dto.Quantity)
            {
                throw new InvalidOperationException("Stock insuficiente para el traspaso");
            }

            var targetStock = await _context.ProductWarehouses
                .FirstOrDefaultAsync(pw => pw.ProductId == dto.ProductId && pw.WarehouseId == dto.TargetWarehouseId);

            if (targetStock == null)
            {
                targetStock = new ProductWarehouse
                {
                    ProductId = dto.ProductId,
                    WarehouseId = dto.TargetWarehouseId,
                    CurrentStock = 0
                };
                _context.ProductWarehouses.Add(targetStock);
            }

            sourceStock.CurrentStock -= dto.Quantity;
            targetStock.CurrentStock += dto.Quantity;

            var outMovement = new InventoryMovements
            {
                ProductId = dto.ProductId,
                WarehouseId = dto.SourceWarehouseId,
                MovementType = "OUT",
                Quantity = dto.Quantity,
                Description = "Transfer OUT"
            };

            var inMovement = new InventoryMovements
            {
                ProductId = dto.ProductId,
                WarehouseId = dto.TargetWarehouseId,
                MovementType = "IN",
                Quantity = dto.Quantity,
                Description = "Transfer IN"
            };

            _context.InventoryMovements.Add(outMovement);
            _context.InventoryMovements.Add(inMovement);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            await _context.Entry(outMovement).Reference(m => m.Product).LoadAsync();
            await _context.Entry(outMovement).Reference(m => m.Warehouse).LoadAsync();
            await _context.Entry(inMovement).Reference(m => m.Product).LoadAsync();
            await _context.Entry(inMovement).Reference(m => m.Warehouse).LoadAsync();

            return new[]
            {
                _mapper.Map<InventoryReportDTO>(outMovement),
                _mapper.Map<InventoryReportDTO>(inMovement)
            };
        }

        public async Task<DashboardStatsDTO> GetDashboardStatsAsync()
        {
            var totalProducts = await _context.Products.CountAsync(p => !p.IsDeleted);
            var totalStock = await _context.ProductWarehouses
                .SumAsync(pw => (int?)pw.CurrentStock) ?? 0;

            var lowStockProducts = await _context.ProductWarehouses
                .Where(pw => !pw.Product.IsDeleted)
                .GroupBy(pw => new { pw.ProductId, pw.Product.Name, pw.Product.MinStock })
                .Select(g => new LowStockProductDTO
                {
                    Name = g.Key.Name,
                    MinStock = g.Key.MinStock,
                    CurrentStock = g.Sum(x => x.CurrentStock)
                })
                .Where(dto => dto.CurrentStock <= dto.MinStock)
                .ToListAsync();

            return new DashboardStatsDTO
            {
                TotalProducts = totalProducts,
                TotalStock = totalStock,
                LowStockCount = lowStockProducts.Count,
                LowStockProducts = lowStockProducts
            };
        }

        public async Task<IEnumerable<InventoryStockDTO>> GetStockAsync(int? productId, int? warehouseId)
        {
            var query = _context.ProductWarehouses
                .Include(pw => pw.Product)
                .Include(pw => pw.Warehouse)
                .Where(pw => !pw.Product.IsDeleted)
                .AsQueryable();

            if (productId.HasValue)
            {
                query = query.Where(pw => pw.ProductId == productId.Value);
            }

            if (warehouseId.HasValue)
            {
                query = query.Where(pw => pw.WarehouseId == warehouseId.Value);
            }

            return await query
                .OrderBy(pw => pw.Product.Name)
                .ThenBy(pw => pw.Warehouse.Name)
                .Select(pw => new InventoryStockDTO
                {
                    ProductId = pw.ProductId,
                    ProductName = pw.Product.Name,
                    WarehouseId = pw.WarehouseId,
                    WarehouseName = pw.Warehouse.Name,
                    CurrentStock = pw.CurrentStock,
                    MinStock = pw.Product.MinStock
                })
                .ToListAsync();
        }
    }
}
