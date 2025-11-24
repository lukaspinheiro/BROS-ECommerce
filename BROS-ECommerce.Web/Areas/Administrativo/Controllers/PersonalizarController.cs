using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers;

[Area("Administrativo")]
[Route("Administrativo/Personalizar")]
public class PersonalizarController : BaseAdminController
{
    public IActionResult Index()
    {
        return View();
    }
}
