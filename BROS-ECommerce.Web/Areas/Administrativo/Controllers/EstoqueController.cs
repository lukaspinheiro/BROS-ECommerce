using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Services.Helpers;
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
                var estoques = await _serviceEstoque.ObterTodosAsync();

                var produtosTabela = estoques.Select(e => new TabelaEstoqueViewModel
                {
                    IdEstoque = e.IdEstoque,
                    IdProduto = e.IdProduto,
                    Nome = e.Produto?.Nome ?? "[Produto não encontrado]",
                    Quantidade = e.Quantidade,
                    UltimaAtualizacao = e.UltimaAtualizacao
                }).ToList();

                var produtos = await _serviceProduto.ObterTodosAsync();

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

        [HttpPost("CadastrarProdutoNoEstoque")]
        public async Task<IActionResult> CadastrarProdutoNoEstoque(IndexEstoqueViewModel indexEstoqueViewModel)
        {
            try
            {
                var idProduto = indexEstoqueViewModel.cadastrarEstoqueViewModel.IdProduto;
                var novaQuantidade = indexEstoqueViewModel.cadastrarEstoqueViewModel.Quantidade;

                var produtoExistente = await _serviceEstoque.ObterPorIdProdutoAsync(indexEstoqueViewModel.cadastrarEstoqueViewModel.IdProduto);

                if (produtoExistente != null)
                {
                    await _serviceEstoque.AtualizarQuantidadeAsync(idProduto, novaQuantidade);
                    TempData["Sucesso"] = "Produto já existente: quantidade atualizada com sucesso!";
                }
                else
                {
                    var produtoNoEstoque = new CadastrarEstoqueViewModel
                    {
                        IdEstoque = Guid.NewGuid(),
                        IdProduto = indexEstoqueViewModel.cadastrarEstoqueViewModel.IdProduto,
                        Quantidade = indexEstoqueViewModel.cadastrarEstoqueViewModel.Quantidade,
                        UltimaAtualizacao = TimeHelper.AgoraPortoVelho()
                    };
                    await _serviceEstoque.AdicionarProdutoNoEstoqueAsync(produtoNoEstoque);
                    TempData["Sucesso"] = "Produto adicionado no estoque com sucesso!";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao adicionar/atualizar produto ao estoque: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost("AlterarQuantidadeEstoque")]
        public async Task<IActionResult> AlterarQuantidadeEstoque(Guid idProduto, int novaQuantidade)
        {
            try
            {
                await _serviceEstoque.AtualizarQuantidadeAsync(idProduto, novaQuantidade);
                TempData["Sucesso"] = "Quantidade atualizada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao atualizar quantidade: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
