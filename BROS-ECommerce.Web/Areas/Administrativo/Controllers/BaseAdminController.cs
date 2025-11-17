using BROS_ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers
{
    [Area("Administrativo")]
    [Authorize]
    public abstract class BaseAdminController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var perfilClaim = User.Claims.FirstOrDefault(c => c.Type == "perfil")?.Value;

            if (perfilClaim == null)
            {
                context.Result = RedirectToAction("Login", "Autenticacao", new { area = "" });
                return;
            }

            int.TryParse(perfilClaim, out int perfil);

            if (perfil != (int)EPerfil.SuperAdmin && perfil != (int)EPerfil.TenantAdmin)
            {
                context.Result = RedirectToAction("SemPermissao", "Erros", new { area = "" });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}