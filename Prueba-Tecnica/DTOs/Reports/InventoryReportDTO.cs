namespace Prueba_Tecnica.DTOs.Reports;

public class InventoryReportDTO
{
    public int ProductCode { get; set; }
    public string ProductDescription { get; set; } = null!;
    public int MovementNumber { get; set; }
    public string MovementType { get; set; } = null!;
    public string Warehouse { get; set; } = null!;
    public int Quantity { get; set; }
}