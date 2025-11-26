using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Pedido;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BROS_ECommerce.Web.Controllers
{
    [Authorize]
    [Route("pedido")]
    public class PedidoController : Controller
    {
        private readonly IServicePedidoIntegracao _servicePedidoIntegracao;

        public PedidoController(IServicePedidoIntegracao servicePedidoIntegracao)
        {
            _servicePedidoIntegracao = servicePedidoIntegracao;
        }

        [HttpGet("sucesso")]
        [AllowAnonymous] // Permite acesso sem login
        public async Task<IActionResult> Sucesso(
            [FromQuery] string? payment_id = null,
            [FromQuery] string? collection_id = null,
            [FromQuery] string? session_id = null)
        {
            try
            {
                Console.WriteLine($"══════════════════════════════════════════════════════");
                Console.WriteLine($"🎉 PÁGINA DE SUCESSO - PAGAMENTO APROVADO");
                Console.WriteLine($"══════════════════════════════════════════════════════");
                Console.WriteLine($"📋 Parâmetros recebidos:");
                Console.WriteLine($"   - payment_id: {payment_id ?? "null"}");
                Console.WriteLine($"   - collection_id: {collection_id ?? "null"}");
                Console.WriteLine($"   - session_id: {session_id ?? "null"}");

                var idUsuario = ObterIdUsuarioLogado();

                if (idUsuario.HasValue)
                {
                    var pedidos = await _servicePedidoIntegracao.ServicePedido.ObterPedidosPorUsuarioAsync(idUsuario.Value);
                    var ultimoPedido = pedidos.OrderByDescending(p => p.DataPedido).FirstOrDefault();

                    if (ultimoPedido != null)
                    {
                        Console.WriteLine($"✅ Pedido encontrado:");
                        Console.WriteLine($"   - Número: {ultimoPedido.NumeroPedido}");
                        Console.WriteLine($"   - Status: {ultimoPedido.Status}");
                        Console.WriteLine($"   - Valor: R$ {ultimoPedido.ValorTotal:F2}");
                        Console.WriteLine($"══════════════════════════════════════════════════════");

                        ViewBag.NumeroPedido = ultimoPedido.NumeroPedido;
                        ViewBag.ValorTotal = ultimoPedido.ValorTotal;
                        ViewBag.DataPedido = ultimoPedido.DataPedido;
                        ViewBag.Status = ultimoPedido.Status;
                        ViewBag.PaymentId = payment_id ?? collection_id ?? session_id;

                        return View(ultimoPedido);
                    }
                }

                // Se não encontrar pedido, mostra página genérica
                Console.WriteLine($"⚠️ Nenhum pedido encontrado, mostrando página genérica");
                Console.WriteLine($"══════════════════════════════════════════════════════");

                ViewBag.NumeroPedido = "Processando...";
                ViewBag.PaymentId = payment_id ?? collection_id ?? session_id;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERRO na página de sucesso: {ex.Message}");
                Console.WriteLine($"══════════════════════════════════════════════════════");

                ViewBag.NumeroPedido = "Erro ao processar";
                return View();
            }
        }

        [HttpGet("cancelado")]
        [AllowAnonymous]
        public IActionResult Cancelado()
        {
            Console.WriteLine($"══════════════════════════════════════════════════════");
            Console.WriteLine($"❌ PÁGINA DE CANCELAMENTO - PAGAMENTO CANCELADO");
            Console.WriteLine($"══════════════════════════════════════════════════════");

            return View();
        }

        [HttpGet("meus-pedidos")]
        public async Task<IActionResult> MeusPedidos()
        {
            var idUsuario = ObterIdUsuarioLogado();

            if (!idUsuario.HasValue)
            {
                Console.WriteLine($"❌ Usuário não autenticado, redirecionando para login");
                return RedirectToAction("Login", "Autenticacao");
            }

            var pedidos = await _servicePedidoIntegracao.ServicePedido.ObterPedidosPorUsuarioAsync(idUsuario.Value);

            Console.WriteLine($"✅ Carregando {pedidos.Count()} pedidos do usuário");

            return View(pedidos.OrderByDescending(p => p.DataPedido));
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
