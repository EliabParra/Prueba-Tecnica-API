using System.ComponentModel.DataAnnotations;

namespace Prueba_Tecnica.DTOs.Products;

public class UpdateProductDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Price { get; set; }

    [Required]
    public string UnitOfMeasure { get; set; } = null!;

    [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo")]
    public int MinStock { get; set; }

    [Required]
    public int CategoryId { get; set; }
}