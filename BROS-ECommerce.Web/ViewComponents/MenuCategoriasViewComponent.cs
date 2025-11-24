using BROS_ECommerce.Core.Interfaces;
using BROS_ECommerce.Services.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.ViewComponents
{
    public class MenuCategoriasViewComponent : ViewComponent
    {
        private readonly IServiceCategoria _serviceCategoria;
        private readonly ITenantAtualService _tenantService;

        public MenuCategoriasViewComponent(
            IServiceCategoria serviceCategoria,
            ITenantAtualService tenantService)
        {
            _serviceCategoria = serviceCategoria;
            _tenantService = tenantService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categorias = await _serviceCategoria.ListarParaMenuAsync();

            ViewBag.CorTexto = _tenantService.TenantAtual?.CorTextoMenuInferior;

            return View(categorias);
        }
    }
}
