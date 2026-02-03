namespace Prueba_Tecnica.DTOs.Auth;

public class AuthResponseDTO
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}
