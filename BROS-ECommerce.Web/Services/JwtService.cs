using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BROS_ECommerce.Domain.Entities; // ou DTO se preferir
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BROS_ECommerce.Web.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("cpf", user.Cpf),
                new Claim("genero", user.Genero),
                new Claim("dataNascimento", user.Nascimento.ToString("yyyy-MM-dd")),
                new Claim("dataLogin", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
