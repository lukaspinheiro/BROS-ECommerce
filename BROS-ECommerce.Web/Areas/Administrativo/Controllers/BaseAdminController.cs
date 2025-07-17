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
            var email = User.Identity?.Name;
            var emailAdmin = "admin@gymbros.com";

            if (email != emailAdmin)
            {
                context.Result = RedirectToAction("Login", "Autenticacao", new { area = "" });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}