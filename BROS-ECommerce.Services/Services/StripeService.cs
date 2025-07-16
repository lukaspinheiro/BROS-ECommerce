using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Carrinho;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

public class StripeService : IStripeService
{
    public StripeService(IConfiguration config)
    {
        StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
    }

    public async Task<string> CriarSessaoCheckoutAsync(CarrinhoViewModel carrinho)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = carrinho.Itens.Select(item => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "brl",
                    UnitAmountDecimal = item.PrecoUnitario * 100,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Nome
                    }
                },
                Quantity = item.Quantidade
            }).ToList(),
            Mode = "payment",
            SuccessUrl = "https://seusite.com/pedido/sucesso",
            CancelUrl = "https://seusite.com/carrinho"
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        return session.Url;
    }
}
