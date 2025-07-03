using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Estoque;
using BROS_ECommerce.Services.ViewModel.Produto;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers
{
    [Area("Administrativo")]
    [Route("Administrativo/Estoque")]
    public class EstoqueController : Controller
    {
        private readonly IServiceEstoque _serviceEstoque;
        private readonly IServiceProduto _serviceProduto;

        public EstoqueController(IServiceEstoque serviceEstoque, IServiceProduto serviceProduto)
        {
            _serviceEstoque = serviceEstoque;
            _serviceProduto = serviceProduto;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                // 1. Carrega os dados do estoque (entidade)
                var estoques = await _serviceEstoque.ObterTodosAsync();

                // 2. Mapeia para a ViewModel esperada pela view
                var produtosTabela = estoques.Select(e => new TabelaEstoqueViewModel
                {
                    IdEstoque = e.IdEstoque,
                    IdProduto = e.IdProduto,
                    Nome = e.Produto?.Nome ?? "[Produto não encontrado]",
                    Quantidade = e.Quantidade,
                    UltimaAtualizacao = e.UltimaAtualizacao
                }).ToList();


                // 3. Carrega os produtos para o <select>
                var produtos = await _serviceProduto.ObterTodosAsync();

                // 4. Monta o viewmodel completo
                var filtro = new FiltroEstoqueViewModel();
                var viewModel = new IndexEstoqueViewModel(filtro, produtosTabela)
                {
                    Produtos = produtos
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = "Erro ao carregar produtos: " + ex.Message;

                var filtro = new FiltroEstoqueViewModel();
                var viewModel = new IndexEstoqueViewModel(filtro, new List<TabelaEstoqueViewModel>())
                {
                    Produtos = new List<ProdutoViewModel>()
                };

                return View(viewModel);
            }
        }
    }
}
