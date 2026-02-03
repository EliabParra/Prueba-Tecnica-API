using AutoMapper;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Prueba_Tecnica.Data;
using Prueba_Tecnica.DTOs.Reports;
using Prueba_Tecnica.Services.Interfaces;

namespace Prueba_Tecnica.Services
{
    public class ReportService : IReportService
    {
        private readonly InventoryDbContext _context;
        private readonly IMapper _mapper;

        public ReportService(InventoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InventoryReportDTO>> GetInventoryReportAsync(int? productId, int? warehouseId)
        {
            var query = _context.InventoryMovements
                .Include(m => m.Product)
                .Include(m => m.Warehouse)
                .AsQueryable();

            if (productId.HasValue)
            {
                query = query.Where(m => m.ProductId == productId.Value);
            }

            if (warehouseId.HasValue)
            {
                query = query.Where(m => m.WarehouseId == warehouseId.Value);
            }

            var movements = await query
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InventoryReportDTO>>(movements);
        }

        public async Task<byte[]> GetInventoryReportExcelAsync(int? productId, int? warehouseId)
        {
            var report = await GetInventoryReportAsync(productId, warehouseId);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add($"Reporte de Inventario {DateTime.Now:ddMMyyyy}");

            worksheet.Cell(1, 1).Value = "Codigo";
            worksheet.Cell(1, 2).Value = "Descripcion";
            worksheet.Cell(1, 3).Value = "N# Movimiento";
            worksheet.Cell(1, 4).Value = "Tipo Movimiento";
            worksheet.Cell(1, 5).Value = "Almacen";
            worksheet.Cell(1, 6).Value = "Cantidad";

            var row = 2;
            foreach (var item in report)
            {
                worksheet.Cell(row, 1).Value = item.ProductCode;
                worksheet.Cell(row, 2).Value = item.ProductDescription;
                worksheet.Cell(row, 3).Value = item.MovementNumber;
                worksheet.Cell(row, 4).Value = item.MovementType;
                worksheet.Cell(row, 5).Value = item.Warehouse;
                worksheet.Cell(row, 6).Value = item.Quantity;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
