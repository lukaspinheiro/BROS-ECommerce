
using BROS_ECommerce.Domain.Entities;
using System.Security.Claims;

namespace BROS_ECommerce.Web.Services
{
    public interface IAuthenticationService
    {
        Task<bool> SignInAsync(HttpContext httpContext, User user, bool rememberMe = false);
        Task SignOutAsync(HttpContext httpContext);
        ClaimsPrincipal CreateClaimsPrincipal(User user);
        Task<User?> GetCurrentUserAsync(HttpContext httpContext);
        bool IsAuthenticated(HttpContext httpContext);
    }
}