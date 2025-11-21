using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Tenant;
using BROS_ECommerce.Web.Areas.Administrativo.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.AdministrativoSuper.Controllers;

[Area("AdministrativoSuper")]
[Route("AdministrativoSuper/Tenant")]
public class TenantController : BaseSuperAdminController
{
    private readonly IServiceTenant _serviceTenant;
    public TenantController(IServiceTenant serviceTenant)
    {
        _serviceTenant = serviceTenant;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var tenantsTabela = await _serviceTenant.ObterTabelaTenantsAsync();
            var filtro = new FiltroTenantViewModel();
            var viewModel = new IndexTenantViewModel(filtro, tenantsTabela.ToList());

            return View(viewModel);
        }
        catch (Exception ex)
        {
            ViewBag.Erro = "Erro ao carregar Tenants: " + ex.Message;
            var filtro = new FiltroTenantViewModel();
            var tabelaVazia = new List<TabelaTenantViewModel>();
            var viewModel = new IndexTenantViewModel(filtro, tabelaVazia);
            return View(viewModel);
        }
    }

    [HttpPost("CadastrarTenant")]
    public async Task<IActionResult> CadastrarTenant(IndexTenantViewModel indexTenantViewModel)
    {
        try
        {
            var cadastrarTenant = indexTenantViewModel.CadastrarTenant;

            await _serviceTenant.CadastrarTenantAsync(indexTenantViewModel);
            TempData["Sucesso"] = "Tenant cadastrado com sucesso!";
        }
        catch (Exception ex)
        {
            TempData["Erro"] = $"Erro ao cadastrar Tenant: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("EditarTenant")]
    public async Task<IActionResult> EditarTenant(IndexTenantViewModel indexTenantViewModel)
    {
        try
        {
            await _serviceTenant.EditarTenantAsync(indexTenantViewModel);
            TempData["Sucesso"] = "Loja editada com sucesso!";
        }
        catch (Exception ex)
        {
            TempData["Erro"] = "Erro ao editar: " + ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
