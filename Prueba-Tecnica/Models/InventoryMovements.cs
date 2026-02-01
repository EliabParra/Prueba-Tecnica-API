using System.ComponentModel.DataAnnotations;

namespace Prueba_Tecnica.Models;

public class InventoryMovements
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int WarehouseId { get; set; }

    [Required]
    public string MovementType { get; set; } = null!;

    [Required]
    public int Quantity { get; set; }

    [Required]
    public DateTime MovementDate { get; set; } = DateTime.Now;

    public string? Description { get; set; }

    public virtual Product Product { get; set; } = null!;
    public virtual Warehouse Warehouse { get; set; } = null!;
}