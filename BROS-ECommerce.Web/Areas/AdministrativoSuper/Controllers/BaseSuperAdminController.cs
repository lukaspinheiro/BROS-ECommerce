using BROS_ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BROS_ECommerce.Web.Areas.AdministrativoSuper.Controllers
{
    [Area("AdministrativoSuper")]
    [Authorize]
    public abstract class BaseSuperAdminController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var perfilClaim = User.Claims.FirstOrDefault(c => c.Type == "perfil")?.Value;

            if (perfilClaim == null || !int.TryParse(perfilClaim, out int perfil))
            {
                context.Result = RedirectToAction("Login", "Autenticacao", new { area = "" });
                return;
            }

            if (perfil != (int)EPerfil.SuperAdmin)
            {
                context.Result = RedirectToAction("SemPermissao", "Erros", new { area = "" });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
