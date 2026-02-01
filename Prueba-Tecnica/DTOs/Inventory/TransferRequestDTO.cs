namespace Prueba_Tecnica.DTOs.Inventory;

public class TransferRequestDTO
{
    public int ProductId { get; set; }
    public int SourceWarehouseId { get; set; }
    public int TargetWarehouseId { get; set; }
    public int Quantity { get; set; }
}