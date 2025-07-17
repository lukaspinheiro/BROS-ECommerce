using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        Console.WriteLine($"[PAGAMENTO] Método: {metodo}");
        Console.WriteLine($"[PAGAMENTO] Dados Contato: {dadosContato}");
        Console.WriteLine($"[PAGAMENTO] Dados Endereço: {dadosEndereco}");
        Console.WriteLine($"[PAGAMENTO] Carrinho ID: {carrinho.IdCarrinho}");
        Console.WriteLine($"[PAGAMENTO] Total: R$ {carrinho.ValorTotal:F2}");

        try
        {
            
            string redirectUrl;

            if (metodo == "stripe")
            {
                redirectUrl = await _stripeService.CriarSessaoCheckoutAsync(carrinho);
            }
            else if (metodo == "mercadopago")
            {
                redirectUrl = await _mercadoPagoService.CriarPreferenciaAsync(carrinho);
            }
            else
            {
                Console.WriteLine($"[PAGAMENTO] Método inválido: {metodo}");
                return BadRequest("Método inválido");
            }

            var observacoes = $"Contato: {dadosContato} | Endereço: {dadosEndereco}";
            var pedido = await _servicePedidoIntegracao.FinalizarPedidoComPagamentoAsync(idUsuario.Value, metodo, observacoes);

            Console.WriteLine($"[PAGAMENTO] Pedido criado: {pedido.NumeroPedido}");
            Console.WriteLine($"[PAGAMENTO] Redirecionando para: {redirectUrl}");
            return Redirect(redirectUrl);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PAGAMENTO] Erro: {ex.Message}");
            return BadRequest($"Erro ao processar pagamento: {ex.Message}");
        }
    }

    [HttpGet("sucesso")]
    public async Task<IActionResult> PagamentoSucesso(string? session_id = null)
    {
        try
        {
            var idUsuario = ObterIdUsuarioLogado();
            if (idUsuario.HasValue)
            {
                
                var pedidos = await _servicePedidoIntegracao.ServicePedido.ObterPedidosPorUsuarioAsync(idUsuario.Value);
                var ultimoPedido = pedidos.OrderByDescending(p => p.DataPedido).FirstOrDefault();

                if (ultimoPedido != null)
                {
                    await _servicePedidoIntegracao.ServicePedido.AtualizarStatusPedidoAsync(ultimoPedido.IdPedido, "Confirmado");
                }
            }

            return RedirectToAction("Sucesso", "Pedido", new { session_id });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PAGAMENTO] Erro no sucesso: {ex.Message}");
            return RedirectToAction("Sucesso", "Pedido", new { session_id });
        }
    }

    [HttpGet("cancelado")]
    public async Task<IActionResult> PagamentoCancelado()
    {
        try
        {
            var idUsuario = ObterIdUsuarioLogado();
            if (idUsuario.HasValue)
            {
                
                var pedidos = await _servicePedidoIntegracao.ServicePedido.ObterPedidosPorUsuarioAsync(idUsuario.Value);
                var ultimoPedido = pedidos.OrderByDescending(p => p.DataPedido).FirstOrDefault();

                if (ultimoPedido != null && ultimoPedido.Status == "Processando")
                {
                    await _servicePedidoIntegracao.ServicePedido.CancelarPedidoAsync(ultimoPedido.IdPedido, "Pagamento cancelado pelo usuário");
                }
            }

            return RedirectToAction("Cancelado", "Pedido");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PAGAMENTO] Erro no cancelamento: {ex.Message}");
            return RedirectToAction("Cancelado", "Pedido");
        }
    }

    [HttpPost("webhook/stripe")]
    public async Task<IActionResult> WebhookStripe()
    {
       
        return Ok();
    }

    [HttpPost("webhook/mercadopago")]
    public async Task<IActionResult> WebhookMercadoPago()
    {
       
        return Ok();
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