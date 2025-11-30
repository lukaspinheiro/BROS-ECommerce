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
        public async Task<IActionResult> Detalhes(string slug)
        {
            var produto = _serviceProduto.ObterPorSlug(slug);
            if (produto == null)
                return NotFound();

            ViewBag.CardsProdutos = await _serviceProduto.ObterTabelaProdutosAsync();
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
        public async Task<IActionResult> BuscarPorCategoria(string nomeCategoria, string descricao)
        {
            var produtos = await _serviceProduto.BuscarPorCategoriaAsync(nomeCategoria);

            ViewBag.DescricaoCategoria = descricao;

            return View("Filtrar", produtos);
        }


    }
}
