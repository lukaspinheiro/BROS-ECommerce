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
    public async Task<IActionResult> IniciarPagamento([FromForm] string metodo)
    {
        var idUsuario = ObterIdUsuarioLogado();
        if (!idUsuario.HasValue)
            return BadRequest("Usuário não autenticado");

        var carrinho = await _serviceCarrinho.ObterCarrinhoUsuarioAsync(idUsuario.Value);
        if (carrinho == null || !carrinho.TemItens)
            return BadRequest("Carrinho vazio");

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
            return BadRequest("Método inválido");
        }

        return Redirect(redirectUrl);
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
