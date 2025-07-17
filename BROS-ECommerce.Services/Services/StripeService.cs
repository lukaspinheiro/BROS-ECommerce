using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Carrinho;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

public class StripeService : IStripeService
{
    private readonly IConfiguration _config;

    public StripeService(IConfiguration config)
    {
        _config = config;
        StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
    }

    public async Task<string> CriarSessaoCheckoutAsync(CarrinhoViewModel carrinho)
    {
        var baseUrl = _config["App:BaseUrl"] ?? "https://localhost:8081";

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
                        Name = item.Nome,
                        Description = $"Produto: {item.Nome}",
                        Images = !string.IsNullOrEmpty(item.ImagemUrl) && !item.ImagemUrl.StartsWith("/")
                            ? new List<string> { item.ImagemUrl }
                            : new List<string> { $"{baseUrl}{item.ImagemUrl}" }
                    }
                },
                Quantity = item.Quantidade
            }).ToList(),
            Mode = "payment",
            SuccessUrl = $"{baseUrl}/pedido/sucesso?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{baseUrl}/carrinho",
            CustomerEmail = null,
            AllowPromotionCodes = true,
            BillingAddressCollection = "required",
            ShippingAddressCollection = new SessionShippingAddressCollectionOptions
            {
                AllowedCountries = new List<string> { "BR" }
            },
            Locale = "pt-BR"
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        Console.WriteLine($"[STRIPE] Sessão criada: {session.Id}");
        Console.WriteLine($"[STRIPE] URL de pagamento: {session.Url}");

        return session.Url;
    }
}