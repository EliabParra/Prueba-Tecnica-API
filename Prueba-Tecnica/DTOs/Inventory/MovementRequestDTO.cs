namespace Prueba_Tecnica.DTOs.Inventory;

public class MovementRequestDTO
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public string MovementType { get; set; } = null!; // IN o OUT
    public int Quantity { get; set; }
    public string? Description { get; set; }
}