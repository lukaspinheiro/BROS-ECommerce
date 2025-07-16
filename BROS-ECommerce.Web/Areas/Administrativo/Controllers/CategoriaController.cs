using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Categoria;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers
{
    [Area("Administrativo")]
    [Route("Administrativo/Categoria")]
    public class CategoriaController : Controller
    {
        private readonly IServiceCategoria _serviceCategoria;

        public CategoriaController(IServiceCategoria serviceCategoria)
        {
            _serviceCategoria = serviceCategoria;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var categoriasTabela = await _serviceCategoria.ObterTabelaCategoriaAsync();
                var filtro = new FiltroCategoriaViewModel();
                var viewModel = new IndexCategoriaViewModel(filtro, categoriasTabela.ToList());

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = "Erro ao carregar Categorias: " + ex.Message;
                var filtro = new FiltroCategoriaViewModel();
                var tabelaVazia = new List<TabelaCategoriaViewModel>();
                var viewModel = new IndexCategoriaViewModel(filtro, tabelaVazia);
                return View(viewModel);
            }
        }
    }
}
