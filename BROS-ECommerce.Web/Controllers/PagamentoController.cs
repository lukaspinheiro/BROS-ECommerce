using BROS_ECommerce.Services.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("pagamento")]
public class PagamentoController : Controller
{
    private readonly IServiceCarrinho _serviceCarrinho;
    private readonly IStripeService _stripeService;
    private readonly IMercadoPagoService _mercadoPagoService;

    public PagamentoController(IServiceCarrinho serviceCarrinho, IStripeService stripeService, IMercadoPagoService mercadoPagoService)
    {
        _serviceCarrinho = serviceCarrinho;
        _stripeService = stripeService;
        _mercadoPagoService = mercadoPagoService;
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

        string redirectUrl;

        try
        {
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

            Console.WriteLine($"[PAGAMENTO] Redirecionando para: {redirectUrl}");
            return Redirect(redirectUrl);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PAGAMENTO] Erro: {ex.Message}");
            return BadRequest($"Erro ao processar pagamento: {ex.Message}");
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