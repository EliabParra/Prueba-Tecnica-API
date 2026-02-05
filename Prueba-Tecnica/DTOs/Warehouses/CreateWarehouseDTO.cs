using System.ComponentModel.DataAnnotations;

namespace Prueba_Tecnica.DTOs.Warehouses;

public class CreateWarehouseDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "La ubicación es obligatoria.")]
    public string Location { get; set; } = null!;
}
