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

        [HttpGet]
        [Route("buscar")]
        public async Task<IActionResult> Buscar(string busca)
        {
            var produtos = await _serviceProduto.BuscarPorTermoAsync(busca);
            ViewBag.TermoBusca = busca;
            return View("Filtrar", produtos);
        }

        [HttpGet]
        [Route("categoria/{nomeCategoria}")]
        public async Task<IActionResult> BuscarPorCategoria(string nomeCategoria)
        {
            var produtos = await _serviceProduto.BuscarPorCategoriaAsync(nomeCategoria);
            ViewBag.TermoBusca = nomeCategoria; // opcional, para mostrar no título
            return View("Filtrar", produtos);
        }

    }
}
