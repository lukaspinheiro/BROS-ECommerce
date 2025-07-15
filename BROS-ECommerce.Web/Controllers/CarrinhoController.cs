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
                
                Console.WriteLine($"[DEBUG] =================================");
                Console.WriteLine($"[DEBUG] INÍCIO - AdicionarProduto");
                Console.WriteLine($"[DEBUG] Produto ID recebido: '{produtoId}'");
                Console.WriteLine($"[DEBUG] Produto ID string: '{produtoId.ToString()}'");
                Console.WriteLine($"[DEBUG] Produto ID é Guid.Empty: {produtoId == Guid.Empty}");
                Console.WriteLine($"[DEBUG] Quantidade: {quantidade}");
                Console.WriteLine($"[DEBUG] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                var idUsuario = ObterIdUsuarioLogado();
                Console.WriteLine($"[DEBUG] ID Usuario: {idUsuario}");
                Console.WriteLine($"[DEBUG] Usuario autenticado: {idUsuario.HasValue}");

                
                if (produtoId == Guid.Empty)
                {
                    Console.WriteLine($"[ERROR] Produto ID é Guid.Empty");
                    return Json(new
                    {
                        sucesso = false,
                        mensagem = "ID do produto não pode ser vazio"
                    });
                }

                if (quantidade <= 0)
                {
                    Console.WriteLine($"[ERROR] Quantidade inválida: {quantidade}");
                    return Json(new
                    {
                        sucesso = false,
                        mensagem = "Quantidade deve ser maior que zero"
                    });
                }

                Console.WriteLine($"[DEBUG] Validações OK - Chamando service...");
                var carrinho = await _serviceCarrinho.AdicionarProdutoAsync(idUsuario, produtoId, quantidade);

                Console.WriteLine($"[DEBUG] SUCCESS - Produto adicionado!");
                Console.WriteLine($"[DEBUG] Carrinho ID: {carrinho.IdCarrinho}");
                Console.WriteLine($"[DEBUG] Quantidade total: {carrinho.QuantidadeTotal}");
                Console.WriteLine($"[DEBUG] Valor total: {carrinho.ValorTotal:C}");
                Console.WriteLine($"[DEBUG] =================================");

                return Json(new
                {
                    sucesso = true,
                    mensagem = "Produto adicionado ao carrinho com sucesso!",
                    quantidadeItens = carrinho.QuantidadeTotal,
                    redirect = Url.Action("Index", "Carrinho"),
                    carrinhoId = carrinho.IdCarrinho
                });
            }
            catch (ArgumentException ex) when (ex.Message.Contains("Produto não encontrado"))
            {
                Console.WriteLine($"[ERROR] =================================");
                Console.WriteLine($"[ERROR] PRODUTO NÃO ENCONTRADO");
                Console.WriteLine($"[ERROR] Produto ID buscado: '{produtoId}'");
                Console.WriteLine($"[ERROR] Mensagem: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack Trace: {ex.StackTrace}");

            
                Console.WriteLine($"[DEBUG] Tentando buscar produto diretamente...");
                try
                {
                    
                    Console.WriteLine($"[DEBUG] Produto existe? Verifique no banco de dados manualmente");
                }
                catch (Exception debugEx)
                {
                    Console.WriteLine($"[DEBUG] Erro no debug: {debugEx.Message}");
                }

                Console.WriteLine($"[ERROR] =================================");

                return Json(new
                {
                    sucesso = false,
                    mensagem = $"Produto não encontrado no catálogo. ID: {produtoId}"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] =================================");
                Console.WriteLine($"[ERROR] ERRO GERAL");
                Console.WriteLine($"[ERROR] Produto ID: '{produtoId}'");
                Console.WriteLine($"[ERROR] Erro: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException?.Message}");
                Console.WriteLine($"[ERROR] =================================");

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
                    return Json(new { itens = new List<object>(), total = 0, quantidadeTotal = 0 });
                }

                var response = new
                {
                    itens = carrinho.Itens.Select(item => new
                    {
                        idProduto = item.IdProduto,
                        nome = item.Nome,
                        quantidade = item.Quantidade,
                        precoUnitario = item.PrecoUnitario,
                        imagemUrl = item.ImagemUrl,
                        subtotal = item.Subtotal
                    }),
                    total = carrinho.ValorTotal,
                    quantidadeTotal = carrinho.QuantidadeTotal
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
                    carrinho = new
                    {
                        itens = carrinho.Itens.Select(item => new
                        {
                            idProduto = item.IdProduto,
                            nome = item.Nome,
                            quantidade = item.Quantidade,
                            precoUnitario = item.PrecoUnitario,
                            imagemUrl = item.ImagemUrl,
                            subtotal = item.Subtotal
                        }),
                        total = carrinho.ValorTotal,
                        quantidadeTotal = carrinho.QuantidadeTotal
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

                object carrinhoResponse;

                if (carrinhoAtualizado == null || !carrinhoAtualizado.TemItens)
                {
                    carrinhoResponse = new { itens = new List<object>(), total = 0m, quantidadeTotal = 0 };
                }
                else
                {
                    carrinhoResponse = new
                    {
                        itens = carrinhoAtualizado.Itens.Select(item => new
                        {
                            idProduto = item.IdProduto,
                            nome = item.Nome,
                            quantidade = item.Quantidade,
                            precoUnitario = item.PrecoUnitario,
                            imagemUrl = item.ImagemUrl,
                            subtotal = item.Subtotal
                        }),
                        total = carrinhoAtualizado.ValorTotal,
                        quantidadeTotal = carrinhoAtualizado.QuantidadeTotal
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

        [HttpGet]
        [Route("Carrinho/Checkout")]
        public async Task<IActionResult> CheckoutPage()
        {
            try
            {
                var idUsuario = ObterIdUsuarioLogado();
                var carrinho = idUsuario.HasValue
                    ? await _serviceCarrinho.ObterCarrinhoUsuarioAsync(idUsuario.Value)
                    : null;

                if (carrinho == null || !carrinho.TemItens)
                {
                    TempData["Aviso"] = "Adicione produtos ao carrinho antes de finalizar a compra.";
                    return RedirectToAction("Index");
                }

                return View(carrinho);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao carregar página de checkout: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        
        [HttpGet]
        public async Task<IActionResult> TestarProduto(Guid produtoId)
        {
            try
            {
                Console.WriteLine($"[TEST] =================================");
                Console.WriteLine($"[TEST] Testando existência do produto");
                Console.WriteLine($"[TEST] Produto ID: {produtoId}");
                Console.WriteLine($"[TEST] Produto ID string: '{produtoId.ToString()}'");
                Console.WriteLine($"[TEST] Produto ID é válido: {produtoId != Guid.Empty}");
                Console.WriteLine($"[TEST] =================================");

                try
                {
                    Console.WriteLine($"[TEST] Simulando chamada do ServiceCarrinho...");
                    var idUsuario = ObterIdUsuarioLogado();

                    Console.WriteLine($"[TEST] ID Usuario obtido: {idUsuario}");
                    Console.WriteLine($"[TEST] Chamando AdicionarProdutoAsync...");

                    

                    Console.WriteLine($"[TEST] Teste concluído sem erro inicial");
                }
                catch (Exception serviceEx)
                {
                    Console.WriteLine($"[TEST] Erro no service: {serviceEx.Message}");
                    Console.WriteLine($"[TEST] Stack trace: {serviceEx.StackTrace}");
                }

                return Json(new
                {
                    produtoId = produtoId,
                    produtoIdString = produtoId.ToString(),
                    produtoIdValido = produtoId != Guid.Empty,
                    mensagem = "Verifique o console para ver os logs do teste",
                    timestamp = DateTime.Now,
                    guidFormatValido = System.Text.RegularExpressions.Regex.IsMatch(
                        produtoId.ToString(),
                        @"^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase
                    )
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TEST ERROR] {ex.Message}");
                return Json(new { erro = ex.Message, stackTrace = ex.StackTrace });
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
    }
}