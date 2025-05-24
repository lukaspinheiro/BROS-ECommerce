using BROS_ECommerce.Services.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IServiceProduto _serviceProduto;

        public ProdutoController(IServiceProduto serviceProduto)
        {
            _serviceProduto = serviceProduto;
        }

        [Route("Produto/{slug}")]
        public ActionResult Detalhes(string slug)
        {
            var produto = _serviceProduto.ObterPorSlug(slug);
            if (produto == null)
                return NotFound();

            return View(produto);
        }
    }
}
