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

        public EstoqueController(IServiceEstoque serviceEstoque)
        {
            _serviceEstoque = serviceEstoque;
        }
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var produtosTabela = await _serviceEstoque.ObterTabelaEstoqueAsync();
                var filtro = new FiltroEstoqueViewModel();
                var viewModel = new IndexEstoqueViewModel(filtro, produtosTabela.ToList());

                return View(viewModel);
            }
            catch (Exception ex)
            {

                ViewBag.Erro = "Erro ao carregar produtos: " + ex.Message;
                var filtro = new FiltroEstoqueViewModel();
                var tabelaVazia = new List<TabelaEstoqueViewModel>();
                var viewModel = new IndexEstoqueViewModel(filtro, tabelaVazia);
                return View(viewModel);
            }
        }
    }
}
