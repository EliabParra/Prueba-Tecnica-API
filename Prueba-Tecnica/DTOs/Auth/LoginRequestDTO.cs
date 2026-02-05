using System.ComponentModel.DataAnnotations;

namespace Prueba_Tecnica.DTOs.Auth;

public class LoginRequestDTO
{
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = null!;
}
