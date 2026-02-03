using Prueba_Tecnica.DTOs.Auth;

namespace Prueba_Tecnica.Services.Interfaces
{
    public interface IAuthService
    {
        AuthResponseDTO? Login(LoginRequestDTO dto);
    }
}
