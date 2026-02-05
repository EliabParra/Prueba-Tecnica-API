using System.ComponentModel.DataAnnotations;

namespace Prueba_Tecnica.DTOs.Products;

public class CreateProductDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
    public string UnitOfMeasure { get; set; } = null!;
    [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo.")]
    public int MinStock { get; set; }
    public int CategoryId { get; set; }
}