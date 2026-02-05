using System.ComponentModel.DataAnnotations;

namespace Prueba_Tecnica.DTOs.Categories
{
    public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
