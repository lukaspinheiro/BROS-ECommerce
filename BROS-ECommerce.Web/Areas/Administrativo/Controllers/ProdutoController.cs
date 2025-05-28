using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Produto;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers
{
    [Area("Administrativo")]
    [Route("Administrativo/Produto")]

    public class ProdutoController : Controller
    {
        private readonly IServiceProduto _serviceProduto;
        public ProdutoController (IServiceProduto serviceProduto)
        {
            _serviceProduto = serviceProduto;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("CadastrarProduto")]
        public async Task<IActionResult> CadastrarProduto(IndexProdutoViewModel indexProdutoViewModel)
        {

            var Produto = new CadastrarProdutoViewModel
            {
                Nome = indexProdutoViewModel.cadastrarProdutoViewModel.Nome,
                Slug = indexProdutoViewModel.cadastrarProdutoViewModel.Slug,
                TituloDescricao = indexProdutoViewModel.cadastrarProdutoViewModel.TituloDescricao,
                Descricao = indexProdutoViewModel.cadastrarProdutoViewModel.Descricao,
                Preco = indexProdutoViewModel.cadastrarProdutoViewModel.Preco
            };
            await _serviceProduto.Adicionar(Produto);
            return RedirectToAction(nameof(Index));
        }
    }
}
