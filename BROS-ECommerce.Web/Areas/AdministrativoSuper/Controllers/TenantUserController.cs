using BROS_ECommerce.Services.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.AdministrativoSuper.Controllers
{
    public class TenantUserController : BaseSuperAdminController
    {
        private readonly IServiceUser _userService;

        public TenantUserController(IServiceUser userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(string tenantId)
        {
            if (string.IsNullOrWhiteSpace(tenantId))
                return BadRequest("TenantId é obrigatório.");

            var usuarios = await _userService.GetUsersByTenantAsync(tenantId);

            ViewBag.TenantId = tenantId;

            return View(usuarios);
        }
    }
}
