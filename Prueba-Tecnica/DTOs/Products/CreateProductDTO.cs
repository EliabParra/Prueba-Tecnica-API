using System.ComponentModel.DataAnnotations;

namespace Prueba_Tecnica.DTOs.Products;

public class CreateProductDTO
{
    [Required]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
    [Required]
    public string UnitOfMeasure { get; set; } = null!;
    [Range(0, int.MaxValue)]
    public int MinStock { get; set; }
    public int CategoryId { get; set; }
}