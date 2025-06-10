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

        public ProdutoController(IServiceProduto serviceProduto)
        {
            _serviceProduto = serviceProduto;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var produtosTabela = await _serviceProduto.ObterTabelaProdutosAsync();
                var filtro = new FiltroProdutoViewModel();
                var viewModel = new IndexProdutoViewModel(filtro, produtosTabela.ToList());

                return View(viewModel);
            }
            catch (Exception ex)
            {
                
                ViewBag.Erro = "Erro ao carregar produtos: " + ex.Message;
                var filtro = new FiltroProdutoViewModel();
                var tabelaVazia = new List<TabelaProdutoViewModel>();
                var viewModel = new IndexProdutoViewModel(filtro, tabelaVazia);
                return View(viewModel);
            }
        }

        [HttpPost("CadastrarProduto")]
        public async Task<IActionResult> CadastrarProduto(IndexProdutoViewModel indexProdutoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var produto = new CadastrarProdutoViewModel
                    {
                        Nome = indexProdutoViewModel.cadastrarProdutoViewModel.Nome,
                        Slug = indexProdutoViewModel.cadastrarProdutoViewModel.Slug,
                        TituloDescricao = indexProdutoViewModel.cadastrarProdutoViewModel.TituloDescricao,
                        Descricao = indexProdutoViewModel.cadastrarProdutoViewModel.Descricao,
                        Preco = indexProdutoViewModel.cadastrarProdutoViewModel.Preco
                    };

                    await _serviceProduto.Adicionar(produto);
                    TempData["Sucesso"] = "Produto cadastrado com sucesso!";
                }
                else
                {
                    TempData["Erro"] = "Por favor, preencha todos os campos obrigatórios.";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao cadastrar produto: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("EditarProduto")]
        public async Task<IActionResult> EditarProduto(IndexProdutoViewModel indexProdutoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    var idString = Request.Form["id"].ToString();
                    if (!Guid.TryParse(idString, out var id))
                    {
                        TempData["Erro"] = "ID do produto inválido.";
                        return RedirectToAction(nameof(Index));
                    }

                    var produtoParaAtualizar = new ProdutoViewModel
                    {
                        IdProduto = id,
                        Nome = indexProdutoViewModel.cadastrarProdutoViewModel.Nome,
                        Slug = indexProdutoViewModel.cadastrarProdutoViewModel.Slug,
                        TituloDescricao = indexProdutoViewModel.cadastrarProdutoViewModel.TituloDescricao,
                        Descricao = indexProdutoViewModel.cadastrarProdutoViewModel.Descricao,
                        Preco = indexProdutoViewModel.cadastrarProdutoViewModel.Preco,
                        Imagens = new List<string>()
                    };

                    await _serviceProduto.AtualizarAsync(produtoParaAtualizar);
                    TempData["Sucesso"] = "Produto atualizado com sucesso!";
                }
                else
                {
                    TempData["Erro"] = "Por favor, preencha todos os campos obrigatórios.";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao atualizar produto: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Excluir/{id}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                await _serviceProduto.ExcluirAsync(id);
                TempData["Sucesso"] = "Produto excluído com sucesso!";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao excluir produto: " + ex.Message;
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("TabelaProdutos")]
        public async Task<IActionResult> TabelaProdutos()
        {
            try
            {
                var produtos = await _serviceProduto.ObterTabelaProdutosAsync();
                return PartialView("~/Views/Produto/Partials/_TabelaProduto.cshtml", produtos);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return PartialView("~/Views/Produto/Partials/_TabelaProduto.cshtml", new List<TabelaProdutoViewModel>());
            }
        }
    }
}