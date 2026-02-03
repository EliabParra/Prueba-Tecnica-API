using System.ComponentModel.DataAnnotations;

namespace Prueba_Tecnica.DTOs.Auth;

public class LoginRequestDTO
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
