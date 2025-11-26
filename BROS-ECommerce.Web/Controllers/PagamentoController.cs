using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace BROS_ECommerce.Web.Controllers
{
    [Route("pagamento")]
    public class PagamentoController : Controller
    {
        private readonly IServiceCarrinho _serviceCarrinho;
        private readonly IStripeService _stripeService;
        private readonly IMercadoPagoService _mercadoPagoService;
        private readonly IServicePedidoIntegracao _servicePedidoIntegracao;

        public PagamentoController(
            IServiceCarrinho serviceCarrinho,
            IStripeService stripeService,
            IMercadoPagoService mercadoPagoService,
            IServicePedidoIntegracao servicePedidoIntegracao)
        {
            _serviceCarrinho = serviceCarrinho;
            _stripeService = stripeService;
            _mercadoPagoService = mercadoPagoService;
            _servicePedidoIntegracao = servicePedidoIntegracao;
        }

        [HttpPost("iniciar")]
        public async Task<IActionResult> IniciarPagamento([FromForm] string metodo, [FromForm] string dadosContato = "", [FromForm] string dadosEndereco = "")
        {
            var idUsuario = ObterIdUsuarioLogado();
            if (!idUsuario.HasValue)
                return BadRequest("Usuário não autenticado");

            var carrinho = await _serviceCarrinho.ObterCarrinhoUsuarioAsync(idUsuario.Value);
            if (carrinho == null || !carrinho.TemItens)
                return BadRequest("Carrinho vazio");

            Console.WriteLine($"[PAGAMENTO] ========================================");
            Console.WriteLine($"[PAGAMENTO] Método: {metodo}");
            Console.WriteLine($"[PAGAMENTO] Dados Contato: {dadosContato}");
            Console.WriteLine($"[PAGAMENTO] Dados Endereço: {dadosEndereco}");
            Console.WriteLine($"[PAGAMENTO] Carrinho ID: {carrinho.IdCarrinho}");
            Console.WriteLine($"[PAGAMENTO] Total: R$ {carrinho.ValorTotal:F2}");
            Console.WriteLine($"[PAGAMENTO] ========================================");

            try
            {
                string redirectUrl;

                if (metodo == "stripe")
                {
                    Console.WriteLine($"[PAGAMENTO] Processando com Stripe...");
                    redirectUrl = await _stripeService.CriarSessaoCheckoutAsync(carrinho);
                }
                else if (metodo == "mercadopago")
                {
                    Console.WriteLine($"[PAGAMENTO] Processando com Mercado Pago...");
                    redirectUrl = await _mercadoPagoService.CriarPreferenciaAsync(carrinho);
                }
                else
                {
                    Console.WriteLine($"[PAGAMENTO] ❌ Método inválido: {metodo}");
                    return BadRequest("Método inválido");
                }

                var observacoes = $"Contato: {dadosContato} | Endereço: {dadosEndereco}";
                var pedido = await _servicePedidoIntegracao.FinalizarPedidoComPagamentoAsync(idUsuario.Value, metodo, observacoes);

                Console.WriteLine($"[PAGAMENTO] ✅ Pedido criado: {pedido.NumeroPedido}");
                Console.WriteLine($"[PAGAMENTO] Redirecionando para: {redirectUrl}");

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PAGAMENTO] ❌ ERRO: {ex.Message}");
                Console.WriteLine($"[PAGAMENTO] StackTrace: {ex.StackTrace}");
                return BadRequest($"Erro ao processar pagamento: {ex.Message}");
            }
        }

        [HttpGet("sucesso")]
        public async Task<IActionResult> PagamentoSucesso(
            [FromQuery] string? session_id = null,
            [FromQuery] string? collection_id = null,
            [FromQuery] string? collection_status = null,
            [FromQuery] string? payment_id = null,
            [FromQuery] string? status = null,
            [FromQuery] string? external_reference = null,
            [FromQuery] string? preference_id = null)
        {
            try
            {
                Console.WriteLine($"[PAGAMENTO] ========================================");
                Console.WriteLine($"[PAGAMENTO] RETORNO DE SUCESSO");
                Console.WriteLine($"[PAGAMENTO] session_id: {session_id}");
                Console.WriteLine($"[PAGAMENTO] collection_id: {collection_id}");
                Console.WriteLine($"[PAGAMENTO] collection_status: {collection_status}");
                Console.WriteLine($"[PAGAMENTO] payment_id: {payment_id}");
                Console.WriteLine($"[PAGAMENTO] status: {status}");
                Console.WriteLine($"[PAGAMENTO] external_reference: {external_reference}");
                Console.WriteLine($"[PAGAMENTO] preference_id: {preference_id}");
                Console.WriteLine($"[PAGAMENTO] ========================================");

                var idUsuario = ObterIdUsuarioLogado();
                if (idUsuario.HasValue)
                {
                    var pedidos = await _servicePedidoIntegracao.ServicePedido.ObterPedidosPorUsuarioAsync(idUsuario.Value);
                    var ultimoPedido = pedidos.OrderByDescending(p => p.DataPedido).FirstOrDefault();

                    if (ultimoPedido != null)
                    {
                        Console.WriteLine($"[PAGAMENTO] Atualizando pedido {ultimoPedido.NumeroPedido} para Confirmado");
                        await _servicePedidoIntegracao.ServicePedido.AtualizarStatusPedidoAsync(ultimoPedido.IdPedido, "Confirmado");
                    }
                }

                return RedirectToAction("Sucesso", "Pedido", new { session_id, payment_id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PAGAMENTO] ❌ Erro no sucesso: {ex.Message}");
                return RedirectToAction("Sucesso", "Pedido");
            }
        }

        [HttpGet("cancelado")]
        public async Task<IActionResult> PagamentoCancelado()
        {
            try
            {
                Console.WriteLine($"[PAGAMENTO] ========================================");
                Console.WriteLine($"[PAGAMENTO] PAGAMENTO CANCELADO");
                Console.WriteLine($"[PAGAMENTO] ========================================");

                var idUsuario = ObterIdUsuarioLogado();
                if (idUsuario.HasValue)
                {
                    var pedidos = await _servicePedidoIntegracao.ServicePedido.ObterPedidosPorUsuarioAsync(idUsuario.Value);
                    var ultimoPedido = pedidos.OrderByDescending(p => p.DataPedido).FirstOrDefault();

                    if (ultimoPedido != null && ultimoPedido.Status == "Processando")
                    {
                        Console.WriteLine($"[PAGAMENTO] Cancelando pedido {ultimoPedido.NumeroPedido}");
                        await _servicePedidoIntegracao.ServicePedido.CancelarPedidoAsync(ultimoPedido.IdPedido, "Pagamento cancelado pelo usuário");
                    }
                }

                return RedirectToAction("Cancelado", "Pedido");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PAGAMENTO] ❌ Erro no cancelamento: {ex.Message}");
                return RedirectToAction("Cancelado", "Pedido");
            }
        }

        [HttpPost("webhook/stripe")]
        public async Task<IActionResult> WebhookStripe()
        {
            return Ok();
        }

        [HttpPost("webhook/mercadopago")]
        public async Task<IActionResult> WebhookMercadoPago(
            [FromQuery] string? id = null,
            [FromQuery] string? topic = null)
        {
            try
            {
                Console.WriteLine($"[WEBHOOK MP] ========================================");
                Console.WriteLine($"[WEBHOOK MP] Nova notificação recebida");
                Console.WriteLine($"[WEBHOOK MP] Topic: {topic}");
                Console.WriteLine($"[WEBHOOK MP] ID: {id}");
                Console.WriteLine($"[WEBHOOK MP] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    Console.WriteLine($"[WEBHOOK MP] Body: {body}");
                }

                Console.WriteLine($"[WEBHOOK MP] ========================================");

                // TODO: Implementar lógica de processamento do webhook
                // Consultar status do pagamento via API do MercadoPago
                // Atualizar status do pedido no banco de dados

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WEBHOOK MP] ❌ Erro: {ex.Message}");
                return Ok(); // Sempre retornar 200 para evitar retry do MP
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
    }
}
