using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Prueba_Tecnica.DTOs.Auth;
using Prueba_Tecnica.Services.Interfaces;

namespace Prueba_Tecnica.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthResponseDTO? Login(LoginRequestDTO dto)
        {
            var username = _configuration["Jwt:Username"];
            var password = _configuration["Jwt:Password"];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("JWT credentials are not configured.");
            }

            if (!string.Equals(dto.Username, username, StringComparison.Ordinal) ||
                !string.Equals(dto.Password, password, StringComparison.Ordinal))
            {
                return null;
            }

            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expiresMinutes = int.TryParse(_configuration["Jwt:ExpiresMinutes"], out var minutes) ? minutes : 60;

            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException("JWT configuration is missing.");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, dto.Username)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: credentials);

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDTO
            {
                Token = tokenValue,
                ExpiresAt = token.ValidTo
            };
        }
    }
}
