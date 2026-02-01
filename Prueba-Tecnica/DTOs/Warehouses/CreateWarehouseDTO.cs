using System.ComponentModel.DataAnnotations;

namespace Prueba_Tecnica.DTOs.Warehouses;

public class CreateWarehouseDTO
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Location { get; set; } = null!;
}
