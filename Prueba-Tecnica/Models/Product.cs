using System.ComponentModel.DataAnnotations;

namespace Prueba_Tecnica.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string UnitOfMeasure { get; set; } = null!;
    public int MinStock { get; set; }
    public int CategoryId { get; set; }
    public bool IsDeleted { get; set; }

    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<ProductWarehouse> ProductWarehouses { get; set; } = new List<ProductWarehouse>();
}