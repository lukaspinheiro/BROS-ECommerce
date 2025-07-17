using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BROS_ECommerce.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static Guid? ObterIdUsuarioLogado(this Controller controller)
        {
            var userIdClaim = controller.User.FindFirst("user_id")?.Value ??
                             controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return null;
        }
    }
}