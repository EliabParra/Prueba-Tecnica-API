using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prueba_Tecnica.DTOs.Auth;
using Prueba_Tecnica.Services.Interfaces;

namespace Prueba_Tecnica.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<AuthResponseDTO> Login(LoginRequestDTO dto)
        {
            var result = _authService.Login(dto);
            if (result == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
