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
                Console.WriteLine($"[CARRINHO] =================================");
                Console.WriteLine($"[CARRINHO] INÍCIO - AdicionarProduto");
                Console.WriteLine($"[CARRINHO] Produto ID: {produtoId}");
                Console.WriteLine($"[CARRINHO] Quantidade: {quantidade}");
                Console.WriteLine($"[CARRINHO] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                var idUsuario = ObterIdUsuarioLogado();
                Console.WriteLine($"[CARRINHO] ID Usuario: {idUsuario}");

                if (produtoId == Guid.Empty)
                {
                    Console.WriteLine($"[CARRINHO] ❌ Produto ID é vazio");
                    return Json(new
                    {
                        sucesso = false,
                        mensagem = "ID do produto não pode ser vazio"
                    });
                }

                if (quantidade <= 0)
                {
                    Console.WriteLine($"[CARRINHO] ❌ Quantidade inválida: {quantidade}");
                    return Json(new
                    {
                        sucesso = false,
                        mensagem = "Quantidade deve ser maior que zero"
                    });
                }

                Console.WriteLine($"[CARRINHO] Validações OK - Chamando service...");
                var carrinho = await _serviceCarrinho.AdicionarProdutoAsync(idUsuario, produtoId, quantidade);

                Console.WriteLine($"[CARRINHO]  Produto adicionado com sucesso!");
                Console.WriteLine($"[CARRINHO] Carrinho ID: {carrinho.IdCarrinho}");
                Console.WriteLine($"[CARRINHO] Quantidade total: {carrinho.QuantidadeTotal}");
                Console.WriteLine($"[CARRINHO] Valor total: {carrinho.ValorTotal:C}");
                Console.WriteLine($"[CARRINHO] Total de itens únicos: {carrinho.Itens?.Count ?? 0}");

                var itensCarrinho = carrinho.Itens?.Select(item => new CarrinhoItemDto
                {
                    IdProduto = item.IdProduto,
                    Nome = item.Nome,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario,
                    ImagemUrl = item.ImagemUrl,
                    Subtotal = item.Subtotal
                }).ToList() ?? new List<CarrinhoItemDto>();

                var carrinhoResponse = new CarrinhoDto
                {
                    Itens = itensCarrinho,
                    Total = carrinho.ValorTotal,
                    QuantidadeTotal = carrinho.QuantidadeTotal
                };

                Console.WriteLine($"[CARRINHO] Response estruturado com {itensCarrinho.Count} itens");
                Console.WriteLine($"[CARRINHO] =================================");

                return Json(new
                {
                    sucesso = true,
                    mensagem = "Produto adicionado ao carrinho com sucesso!",
                    carrinho = carrinhoResponse,
                    quantidadeItens = carrinho.QuantidadeTotal,
                    redirect = Url.Action("Index", "Carrinho"),
                    carrinhoId = carrinho.IdCarrinho
                });
            }
            catch (ArgumentException ex) when (ex.Message.Contains("Produto não encontrado"))
            {
                Console.WriteLine($"[CARRINHO] ❌ PRODUTO NÃO ENCONTRADO");
                Console.WriteLine($"[CARRINHO] Produto ID: {produtoId}");
                Console.WriteLine($"[CARRINHO] Erro: {ex.Message}");

                return Json(new
                {
                    sucesso = false,
                    mensagem = $"Produto não encontrado no catálogo. ID: {produtoId}"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CARRINHO] ❌ ERRO GERAL");
                Console.WriteLine($"[CARRINHO] Produto ID: {produtoId}");
                Console.WriteLine($"[CARRINHO] Erro: {ex.Message}");
                Console.WriteLine($"[CARRINHO] Stack Trace: {ex.StackTrace}");

                return Json(new
                {
                    sucesso = false,
                    mensagem = $"Erro interno: {ex.Message}"
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

        [HttpGet]
        public async Task<IActionResult> ObterCarrinhoSidebar()
        {
            try
            {
                var idUsuario = ObterIdUsuarioLogado();
                var carrinho = idUsuario.HasValue
                    ? await _serviceCarrinho.ObterCarrinhoUsuarioAsync(idUsuario.Value)
                    : null;

                if (carrinho == null || !carrinho.TemItens)
                {
                    return Json(new CarrinhoDto
                    {
                        Itens = new List<CarrinhoItemDto>(),
                        Total = 0,
                        QuantidadeTotal = 0
                    });
                }

                var response = new CarrinhoDto
                {
                    Itens = carrinho.Itens.Select(item => new CarrinhoItemDto
                    {
                        IdProduto = item.IdProduto,
                        Nome = item.Nome,
                        Quantidade = item.Quantidade,
                        PrecoUnitario = item.PrecoUnitario,
                        ImagemUrl = item.ImagemUrl,
                        Subtotal = item.Subtotal
                    }).ToList(),
                    Total = carrinho.ValorTotal,
                    QuantidadeTotal = carrinho.QuantidadeTotal
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { erro = true, mensagem = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarQuantidadeSidebar([FromBody] AtualizarQuantidadeRequest request)
        {
            try
            {
                var idUsuario = ObterIdUsuarioLogado();
                if (!idUsuario.HasValue)
                {
                    return Json(new { sucesso = false, mensagem = "Usuário não autenticado" });
                }

                var carrinho = await _serviceCarrinho.AtualizarQuantidadeProdutoAsync(
                    idUsuario.Value,
                    request.ProdutoId,
                    request.Quantidade
                );

                var response = new
                {
                    sucesso = true,
                    mensagem = "Quantidade atualizada com sucesso!",
                    carrinho = new CarrinhoDto
                    {
                        Itens = carrinho.Itens.Select(item => new CarrinhoItemDto
                        {
                            IdProduto = item.IdProduto,
                            Nome = item.Nome,
                            Quantidade = item.Quantidade,
                            PrecoUnitario = item.PrecoUnitario,
                            ImagemUrl = item.ImagemUrl,
                            Subtotal = item.Subtotal
                        }).ToList(),
                        Total = carrinho.ValorTotal,
                        QuantidadeTotal = carrinho.QuantidadeTotal
                    }
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoverProdutoSidebar([FromBody] RemoverProdutoRequest request)
        {
            try
            {
                var idUsuario = ObterIdUsuarioLogado();
                if (!idUsuario.HasValue)
                {
                    return Json(new { sucesso = false, mensagem = "Usuário não autenticado" });
                }

                var carrinho = await _serviceCarrinho.ObterCarrinhoUsuarioAsync(idUsuario.Value);
                if (carrinho == null)
                {
                    return Json(new { sucesso = false, mensagem = "Carrinho não encontrado" });
                }

                var sucesso = await _serviceCarrinho.RemoverProdutoAsync(carrinho.IdCarrinho, request.ProdutoId);

                if (!sucesso)
                {
                    return Json(new { sucesso = false, mensagem = "Erro ao remover produto" });
                }

                var carrinhoAtualizado = await _serviceCarrinho.ObterCarrinhoUsuarioAsync(idUsuario.Value);

                CarrinhoDto carrinhoResponse;

                if (carrinhoAtualizado == null || !carrinhoAtualizado.TemItens)
                {
                    carrinhoResponse = new CarrinhoDto
                    {
                        Itens = new List<CarrinhoItemDto>(),
                        Total = 0m,
                        QuantidadeTotal = 0
                    };
                }
                else
                {
                    carrinhoResponse = new CarrinhoDto
                    {
                        Itens = carrinhoAtualizado.Itens.Select(item => new CarrinhoItemDto
                        {
                            IdProduto = item.IdProduto,
                            Nome = item.Nome,
                            Quantidade = item.Quantidade,
                            PrecoUnitario = item.PrecoUnitario,
                            ImagemUrl = item.ImagemUrl,
                            Subtotal = item.Subtotal
                        }).ToList(),
                        Total = carrinhoAtualizado.ValorTotal,
                        QuantidadeTotal = carrinhoAtualizado.QuantidadeTotal
                    };
                }

                var response = new
                {
                    sucesso = true,
                    mensagem = "Produto removido com sucesso!",
                    carrinho = carrinhoResponse
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
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

        public class AtualizarQuantidadeRequest
        {
            public Guid ProdutoId { get; set; }
            public int Quantidade { get; set; }
        }

        public class RemoverProdutoRequest
        {
            public Guid ProdutoId { get; set; }
        }

        public class CarrinhoItemDto
        {
            public Guid IdProduto { get; set; }
            public string Nome { get; set; } = string.Empty;
            public int Quantidade { get; set; }
            public decimal PrecoUnitario { get; set; }
            public string ImagemUrl { get; set; } = string.Empty;
            public decimal Subtotal { get; set; }
        }

        public class CarrinhoDto
        {
            public List<CarrinhoItemDto> Itens { get; set; } = new List<CarrinhoItemDto>();
            public decimal Total { get; set; }
            public int QuantidadeTotal { get; set; }
        }
    }
}