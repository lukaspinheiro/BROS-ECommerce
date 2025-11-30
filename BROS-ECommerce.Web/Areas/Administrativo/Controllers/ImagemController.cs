using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Imagem;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers
{
    [Area("Administrativo")]
    [Route("Administrativo/Imagem")]
    public class ImagemController : BaseAdminController
    {
        private readonly IServiceImagem _serviceImagem;

        public ImagemController(IServiceImagem serviceImagem)
        {
            _serviceImagem = serviceImagem;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index(int pagina = 1)
        {
            try
            {
                var imagens = await _serviceImagem.ObterPaginadoAsync(pagina, 12);
                var total = await _serviceImagem.ContarAtivosAsync();
                var tamanhoTotal = await _serviceImagem.ObterTamanhoTotalAsync();

                var viewModel = new IndexImagemViewModel
                {
                    Imagens = imagens,
                    PaginaAtual = pagina,
                    TotalItens = total,
                    ItensPorPagina = 12,
                    TamanhoTotal = tamanhoTotal
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar imagens: {ex.Message}";
                return View(new IndexImagemViewModel());
            }
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IndexImagemViewModel model)
        {
            try
            {
                if (model.CadastrarImagem.Arquivos?.Any() == true)
                {
                    if (model.CadastrarImagem.ValidarArquivos(out var erros))
                    {
                        var ids = await _serviceImagem.AdicionarMultiplasAsync(model.CadastrarImagem);
                        TempData["Sucesso"] = $"{ids.Count} imagem(ns) enviada(s) com sucesso!";
                    }
                    else
                    {
                        TempData["Erro"] = string.Join(", ", erros);
                    }
                }
                else
                {
                    TempData["Erro"] = "Selecione pelo menos uma imagem";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro no upload: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpGet("Detalhes/{id}")]
        public async Task<IActionResult> Detalhes(Guid id)
        {
            try
            {
                var imagem = await _serviceImagem.ObterPorIdAsync(id);
                if (imagem == null)
                {
                    TempData["Erro"] = "Imagem não encontrada";
                    return RedirectToAction("Index");
                }

                return View(imagem);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar imagem: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("Excluir/{id}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                await _serviceImagem.ExcluirAsync(id);
                TempData["Sucesso"] = "Imagem excluída com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao excluir imagem: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost("ExcluirLogicamente/{id}")]
        public async Task<IActionResult> ExcluirLogicamente(Guid id)
        {
            try
            {
                await _serviceImagem.ExcluirLogicamenteAsync(id);
                TempData["Sucesso"] = "Imagem desativada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao desativar imagem: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpGet("SelecionarParaProduto/{idProduto}")]
        public async Task<IActionResult> SelecionarParaProduto(Guid idProduto)
        {
            try
            {
                var imagens = await _serviceImagem.ObterAtivosAsync();
                var imagensProduto = await _serviceImagem.ObterImagensPorProdutoAsync(idProduto);

                ViewBag.IdProduto = idProduto;
                ViewBag.ImagensProduto = imagensProduto.Select(i => i.IdImagem).ToList();

                return View(imagens);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar imagens: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("AssociarAoProduto")]
        public async Task<IActionResult> AssociarAoProduto(Guid idProduto, List<Guid> imagensSelecionadas)
        {
            try
            {
                if (imagensSelecionadas?.Any() == true)
                {
                    await _serviceImagem.AssociarImagensAoProdutoAsync(idProduto, imagensSelecionadas);
                    TempData["Sucesso"] = "Imagens associadas ao produto com sucesso!";
                }
                else
                {
                    TempData["Erro"] = "Selecione pelo menos uma imagem";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao associar imagens: {ex.Message}";
            }

            return RedirectToAction("Index", "Produto");
        }

        [HttpPost("DefinirPrincipal")]
        public async Task<IActionResult> DefinirPrincipal(Guid idProduto, Guid idImagem)
        {
            try
            {
                await _serviceImagem.DefinirImagemPrincipalAsync(idProduto, idImagem);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}