
using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace BROS_ECommerce.Web.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IServiceUser _userService;

        public AuthenticationService(IServiceUser userService)
        {
            _userService = userService;
        }

        public async Task<bool> SignInAsync(HttpContext httpContext, User user, bool rememberMe = false)
        {
            try
            {
                var claims = CreateClaimsPrincipal(user);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(2)
                };

                await httpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claims,
                    authProperties
                );

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public ClaimsPrincipal CreateClaimsPrincipal(User user)
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

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        public async Task<User?> GetCurrentUserAsync(HttpContext httpContext)
        {
            if (!IsAuthenticated(httpContext))
                return null;

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return await _userService.GetUserByIdAsync(userId);
            }

            return null;
        }

        public bool IsAuthenticated(HttpContext httpContext)
        {
            return httpContext.User?.Identity?.IsAuthenticated ?? false;
        }
    }
}