using BROS_ECommerce.Core.Interfaces;
using BROS_ECommerce.Domain.Enums;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Personalizar;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers;

[Area("Administrativo")]
[Route("Administrativo/Personalizar")]
public class PersonalizarController : BaseAdminController
{
    private readonly ITenantAtualService _tenantAtualService;
    private readonly IServiceTenant _tenantService;

    public PersonalizarController(
        ITenantAtualService tenantAtualService,
        IServiceTenant tenantService)
    {
        _tenantAtualService = tenantAtualService;
        _tenantService = tenantService;
    }


    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("SalvarTema")]
    public async Task<IActionResult> SalvarTema(PersonalizarLojaViewModel model)
    {
        var tenant = _tenantAtualService.TenantAtual!;
        await _tenantService.AtualizarTemaAsync(tenant, model);

        TempData["success"] = "Tema atualizado com sucesso!";
        return RedirectToAction("Index");
    }

}
