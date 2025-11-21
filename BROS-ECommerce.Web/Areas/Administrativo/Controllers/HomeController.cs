using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers;

[Area("Administrativo")]
public class HomeController : BaseAdminController
{
    public IActionResult Index()
    {
        return View();
    }
}
