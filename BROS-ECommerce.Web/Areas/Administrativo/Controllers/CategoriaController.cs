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

        [HttpPost("CadastrarCategoria")]
        public async Task<IActionResult> CadastrarCategoria(IndexCategoriaViewModel indexCategoriaViewModel)
        {
            try
            {
                var cadastrarCategoria = indexCategoriaViewModel.cadastrarCategoriaViewModel;

                if (!ModelState.IsValid)
                {
                    TempData["Erro"] = "Todos os campos obrigatórios, devem ser preenchidos corretamente.";
                    return RedirectToAction(nameof(Index));
                }

                await _serviceCategoria.AdicionarCategoriaAsync(cadastrarCategoria);

                TempData["Sucesso"] = "Categoria cadastrada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao cadastrar categoria: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("ExcluirCategoria")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirCategoria(Guid idCategoria)
        {
            try
            {
                await _serviceCategoria.ExcluirAsync(idCategoria);
                TempData["Sucesso"] = "Categoria excluída com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao excluir categoria: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("EditarCategoria")]
        public async Task<IActionResult> EditarCategoria(CadastrarCategoriaViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var idString = Request.Form["IdCategoria"].ToString();

                    if (!Guid.TryParse(idString, out var id))
                    {
                        TempData["Erro"] = "ID da categoria inválido.";
                        return RedirectToAction("Index");
                    }

                    model.IdCategoria = id;

                    await _serviceCategoria.EditarCategoriaAsync(model);

                    TempData["Sucesso"] = "Categoria atualizada com sucesso!";
                }
                else
                {
                    TempData["Erro"] = "Por favor, preencha todos os campos obrigatórios.";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao editar categoria: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

    }
}
