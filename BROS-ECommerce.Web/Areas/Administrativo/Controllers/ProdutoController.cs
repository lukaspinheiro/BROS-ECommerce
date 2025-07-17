using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.CategoriaProduto;
using BROS_ECommerce.Services.ViewModel.Produto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers
{
    [Area("Administrativo")]
    [Route("Administrativo/Produto")]
    public class ProdutoController : Controller
    {
        private readonly IServiceProduto _serviceProduto;
        private readonly IServiceImagem _serviceImagem;
        private readonly IServiceCategoria _serviceCategoria;
        private readonly IServiceCategoriaProduto _serviceCategoriaProduto;

        public ProdutoController(IServiceProduto serviceProduto, IServiceImagem serviceImagem, IServiceCategoria serviceCategoria, IServiceCategoriaProduto serviceCategoriaProduto)
        {
            _serviceProduto = serviceProduto;
            _serviceImagem = serviceImagem;
            _serviceCategoria = serviceCategoria;
            _serviceCategoriaProduto = serviceCategoriaProduto;
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
                var cadastrarProduto = indexProdutoViewModel.cadastrarProdutoViewModel;

                cadastrarProduto.IndiceImagemPrincipal = indexProdutoViewModel.cadastrarProdutoViewModel.IndiceImagemPrincipal;

                if (cadastrarProduto.Arquivos == null || !cadastrarProduto.Arquivos.Any())
                {
                    ModelState.AddModelError("cadastrarProdutoViewModel.Arquivos", "Envie pelo menos uma imagem.");
                }

                if (!ModelState.IsValid)
                {
                    TempData["Erro"] = "Todos os campos obrigatórios, incluindo as imagens, devem ser preenchidos.";
                    return RedirectToAction(nameof(Index));
                }

                await _serviceProduto.AdicionarComImagensAsync(indexProdutoViewModel);

                TempData["Sucesso"] = "Produto cadastrado com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao cadastrar produto: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("ObterDetalhes/{id}")]
        public async Task<IActionResult> ObterDetalhes(Guid id)
        {
            var produto = await _serviceProduto.ObterPorIdAsync(id);
            if (produto == null)
                return NotFound();

            var imagens = produto.ImagensDetalhadas
                .Select(i => new
                {
                    id = i.IdImagem,
                    url = Url.Content(i.CaminhoArquivo),
                    principal = i.Principal
                })
                .ToList();

            var imagemPrincipal = produto.ImagensDetalhadas.FirstOrDefault(i => i.Principal);
            return Json(new
            {
                nome = produto.Nome,
                slug = produto.Slug,
                tituloDescricao = produto.TituloDescricao,
                descricao = produto.Descricao,
                preco = produto.Preco,
                imagens = imagens,
                idImagemPrincipal = imagemPrincipal?.IdImagem.ToString() ?? ""
            });
        }

        [HttpGet("ObterImagensPorProduto/{idProduto}")]
        public async Task<IActionResult> ObterImagensPorProduto(Guid idProduto)
        {
            try
            {
                var imagens = await _serviceImagem.ObterImagensPorProdutoAsync(idProduto);
                var urls = imagens.Select(i => Url.Content(i.CaminhoArquivo)).ToList();

                return Json(urls);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao obter imagens: " + ex.Message });
            }
        }

        [HttpPost("AtualizarProduto")]
        public async Task<IActionResult> AtualizarProduto(IndexProdutoViewModel indexProdutoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var idString = Request.Form["id"].ToString();
                    if (!Guid.TryParse(idString, out var id))
                    {
                        TempData["Erro"] = "ID do produto inválido.";
                        return RedirectToAction("Index");
                    }

                    indexProdutoViewModel.cadastrarProdutoViewModel.IdProduto = id;

                    await _serviceProduto.AtualizarComImagensAsync(indexProdutoViewModel);

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

            return RedirectToAction("Index");
        }

        [HttpPost("Excluir/{id}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                var imagens = await _serviceImagem.ObterImagensPorProdutoAsync(id);
                
                await _serviceProduto.ExcluirAsync(id);
                List<string> errosExclusaoImagens = new();

                foreach (var imagem in imagens)
                {
                    try
                    {
                        await _serviceImagem.ExcluirAsync(imagem.IdImagem);
                    }
                    catch (Exception exImagem)
                    {
                        errosExclusaoImagens.Add($"Erro ao excluir imagem {imagem.IdImagem}: {exImagem.Message}");
                    }
                }

                if (errosExclusaoImagens.Count > 0)
                {
                    TempData["Erro"] = "Produto excluído, mas houve erro(s) ao excluir imagem(ns).";
                    return Json(new { success = false, mensagens = errosExclusaoImagens });
                }

                TempData["Sucesso"] = "Produto e imagens excluídos com sucesso!";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao excluir produto: " + ex.Message;
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("GerenciarImagens/{id}")]
        public async Task<IActionResult> GerenciarImagens(Guid id)
        {
            try
            {
                var produto = await _serviceProduto.ObterPorIdAsync(id);
                if (produto == null)
                {
                    TempData["Erro"] = "Produto não encontrado";
                    return RedirectToAction("Index");
                }

                var todasImagens = await _serviceImagem.ObterAtivosAsync();
                var imagensProduto = await _serviceImagem.ObterImagensPorProdutoAsync(id);

                ViewBag.Produto = produto;
                ViewBag.ImagensProduto = imagensProduto;
                ViewBag.TodasImagens = todasImagens;

                return View();
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar imagens: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("AssociarImagem")]
        public async Task<IActionResult> AssociarImagem(Guid idProduto, Guid idImagem, bool principal = false)
        {
            try
            {
                await _serviceImagem.AssociarImagemAoProdutoAsync(idProduto, idImagem, principal);

                if (principal)
                {
                    await _serviceImagem.DefinirImagemPrincipalAsync(idProduto, idImagem);
                }

                return Json(new { success = true, message = "Imagem associada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("RemoverImagem")]
        public async Task<IActionResult> RemoverImagem(Guid idProduto, Guid idImagem)
        {
            try
            {
                await _serviceImagem.RemoverAssociacaoProdutoAsync(idProduto, idImagem);
                return Json(new { success = true, message = "Imagem removida com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("DefinirImagemPrincipal")]
        public async Task<IActionResult> DefinirImagemPrincipal(Guid idProduto, Guid idImagem)
        {
            try
            {
                await _serviceImagem.DefinirImagemPrincipalAsync(idProduto, idImagem);
                return Json(new { success = true, message = "Imagem principal definida!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("UploadEAssociar/{idProduto}")]
        public async Task<IActionResult> UploadEAssociar(Guid idProduto, List<IFormFile> arquivos, string? altText)
        {
            try
            {
                if (arquivos?.Any() == true)
                {
                    var cadastrarImagem = new BROS_ECommerce.Services.ViewModel.Imagem.CadastrarImagemViewModel
                    {
                        Arquivos = arquivos,
                        AltText = altText
                    };

                    if (cadastrarImagem.ValidarArquivos(out var erros))
                    {
                        var idsImagens = await _serviceImagem.AdicionarMultiplasAsync(cadastrarImagem);
                        await _serviceImagem.AssociarImagensAoProdutoAsync(idProduto, idsImagens);

                        TempData["Sucesso"] = $"{idsImagens.Count} imagem(ns) adicionada(s) ao produto!";
                    }
                    else
                    {
                        TempData["Erro"] = string.Join(", ", erros);
                    }
                }
                else
                {
                    TempData["Erro"] = "Selecione pelo menos um arquivo";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro no upload: {ex.Message}";
            }

            return RedirectToAction("GerenciarImagens", new { id = idProduto });
        }

        [HttpGet("ModalCategorias")]
        public async Task<IActionResult> ModalCategorias(Guid idProduto)
        {
            var produto = await _serviceProduto.ObterPorIdAsync(idProduto);

            if (produto == null)
                return NotFound();

            var categoriasAssociadas = await _serviceCategoriaProduto.ListarPorProdutoAsync(idProduto);
            var todasCategorias = await _serviceCategoria.ListarTodasAsync();

            var categoriasDisponiveis = todasCategorias
                .Where(c => !categoriasAssociadas.Any(a => a.IdCategoria == c.IdCategoria))
                .Select(c => new SelectListItem
                {
                    Value = c.IdCategoria.ToString(),
                    Text = c.NomeCategoria
                }).ToList();

            var viewModel = new ProdutoCategoriasModalViewModel
            {
                IdProduto = idProduto,
                CategoriasDisponiveis = categoriasDisponiveis,
                CategoriasAssociadas = categoriasAssociadas.Select(cp =>
                {
                    var categoria = todasCategorias.FirstOrDefault(c => c.IdCategoria == cp.IdCategoria);
                    return new CategoriaProdutoViewModel
                    {
                        IdCategoriaProduto = cp.IdCategoriaProduto,
                        IdCategoria = cp.IdCategoria,
                        IdProduto = cp.IdProduto,
                        NomeCategoria = categoria?.NomeCategoria ?? string.Empty,
                        DescricaoCategoria = categoria?.Descricao ?? string.Empty,
                        NomeProduto = produto.Nome
                    };
                }).ToList()
            };

            return PartialView("Partials/_ModalCategorias", viewModel);
        }


        [HttpPost("Associar")]
        public async Task<IActionResult> Associar(Guid idProduto, Guid idCategoria)
        {
            var viewModel = new CategoriaProdutoViewModel
            {
                IdCategoria = idCategoria,
                IdProduto = idProduto
            };

            var categoriaProduto = await _serviceCategoriaProduto.AdicionarAsync(viewModel);
            var categoria = await _serviceCategoria.ObterPorIdAsync(idCategoria);

            return Json(new
            {
                idCategoriaProduto = categoriaProduto.IdCategoriaProduto,
                nomeCategoria = categoria.NomeCategoria,
                descricaoCategoria = categoria.Descricao
            });
        }

        [HttpPost("Remover")]
        public async Task<IActionResult> Remover(Guid idCategoriaProduto)
        {
            await _serviceCategoriaProduto.RemoverAsync(idCategoriaProduto);
            return Ok();
        }

    }
}