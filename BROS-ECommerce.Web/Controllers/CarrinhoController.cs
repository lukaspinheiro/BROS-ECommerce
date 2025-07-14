using BROS_ECommerce.Services.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BROS_ECommerce.Web.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly IServiceCarrinho _serviceCarrinho;

        public CarrinhoController(IServiceCarrinho serviceCarrinho)
        {
            _serviceCarrinho = serviceCarrinho;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var idUsuario = ObterIdUsuarioLogado();
                var carrinho = idUsuario.HasValue
                    ? await _serviceCarrinho.ObterCarrinhoUsuarioAsync(idUsuario.Value)
                    : null;

                return View(carrinho);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao carregar carrinho: " + ex.Message;
                return View(null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarProduto(Guid produtoId, int quantidade = 1)
        {
            try
            {
                var idUsuario = ObterIdUsuarioLogado();
                var carrinho = await _serviceCarrinho.AdicionarProdutoAsync(idUsuario, produtoId, quantidade);

                return Json(new
                {
                    sucesso = true,
                    mensagem = "Produto adicionado ao carrinho com sucesso!",
                    quantidadeItens = carrinho.QuantidadeTotal,
                    redirect = Url.Action("Index", "Carrinho")
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    sucesso = false,
                    mensagem = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarQuantidade(Guid carrinhoId, Guid produtoId, int quantidade)
        {
            try
            {
                if (quantidade <= 0)
                {
                    return await RemoverProduto(carrinhoId, produtoId);
                }

                var carrinho = await _serviceCarrinho.AtualizarQuantidadeAsync(carrinhoId, produtoId, quantidade);

                return Json(new
                {
                    sucesso = true,
                    quantidadeItens = carrinho.QuantidadeTotal,
                    valorTotal = carrinho.ValorTotal.ToString("C2")
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    sucesso = false,
                    mensagem = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoverProduto(Guid carrinhoId, Guid produtoId)
        {
            try
            {
                var sucesso = await _serviceCarrinho.RemoverProdutoAsync(carrinhoId, produtoId);

                if (sucesso)
                {
                    var idUsuario = ObterIdUsuarioLogado();
                    var carrinho = idUsuario.HasValue
                        ? await _serviceCarrinho.ObterCarrinhoUsuarioAsync(idUsuario.Value)
                        : null;

                    return Json(new
                    {
                        sucesso = true,
                        mensagem = "Produto removido do carrinho",
                        quantidadeItens = carrinho?.QuantidadeTotal ?? 0,
                        valorTotal = carrinho?.ValorTotal.ToString("C2") ?? "R$ 0,00",
                        temItens = carrinho?.TemItens ?? false
                    });
                }

                return Json(new
                {
                    sucesso = false,
                    mensagem = "Produto não encontrado"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    sucesso = false,
                    mensagem = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> LimparCarrinho(Guid carrinhoId)
        {
            try
            {
                var sucesso = await _serviceCarrinho.LimparCarrinhoAsync(carrinhoId);

                return Json(new
                {
                    sucesso = sucesso,
                    mensagem = sucesso ? "Carrinho limpo com sucesso" : "Erro ao limpar carrinho"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    sucesso = false,
                    mensagem = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> QuantidadeItens()
        {
            try
            {
                var idUsuario = ObterIdUsuarioLogado();
                var quantidade = await _serviceCarrinho.ObterQuantidadeItensAsync(idUsuario);

                return Json(new { quantidade });
            }
            catch
            {
                return Json(new { quantidade = 0 });
            }
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        private Guid? ObterIdUsuarioLogado()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(idClaim, out var id))
                {
                    return id;
                }
            }
            return null;
        }
    }
}