namespace Prueba_Tecnica.DTOs.Inventory;

public class InventoryStockDTO
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = null!;
    public int CurrentStock { get; set; }
    public int MinStock { get; set; }
}
