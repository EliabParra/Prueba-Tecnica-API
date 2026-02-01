using System.ComponentModel.DataAnnotations;

namespace Prueba_Tecnica.DTOs.Categories
{
    public class CreateCategoryDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
