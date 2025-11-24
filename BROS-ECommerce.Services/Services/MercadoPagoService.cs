using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Carrinho;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using Microsoft.Extensions.Configuration;

public class MercadoPagoService : IMercadoPagoService
{
    private readonly IConfiguration _config;

    public MercadoPagoService(IConfiguration config)
    {
        _config = config;
        MercadoPagoConfig.AccessToken = config["MercadoPago:AccessToken"];
    }

    public async Task<string> CriarPreferenciaAsync(CarrinhoViewModel carrinho)
    {
        var baseUrl = _config["App:BaseUrl"] ?? "https://localhost:8081";

        var items = carrinho.Itens.Select(item => new PreferenceItemRequest
        {
            Title = item.Nome,
            Description = $"Produto: {item.Nome} - Quantidade: {item.Quantidade}",
            Quantity = item.Quantidade,
            CurrencyId = "BRL",
            UnitPrice = item.PrecoUnitario,
            PictureUrl = !string.IsNullOrEmpty(item.ImagemUrl) && !item.ImagemUrl.StartsWith("/")
                ? item.ImagemUrl
                : $"{baseUrl}{item.ImagemUrl}",
            CategoryId = "health", 
            Id = item.IdProduto.ToString()
        }).ToList();

        var request = new PreferenceRequest
        {
            Items = items,
            BackUrls = new PreferenceBackUrlsRequest
            {
                Success = $"{baseUrl}/pedido/sucesso",
                Failure = $"{baseUrl}/carrinho",
                Pending = $"{baseUrl}/pedido/pendente"
            },
            AutoReturn = "approved",
            PaymentMethods = new PreferencePaymentMethodsRequest
            {
                ExcludedPaymentMethods = new List<PreferencePaymentMethodRequest>(),
                ExcludedPaymentTypes = new List<PreferencePaymentTypeRequest>(),
                Installments = 12 
            },
            NotificationUrl = $"{baseUrl}/webhooks/mercadopago",
            StatementDescriptor = "GYMBROS SUPPLEMENTS",
            ExternalReference = carrinho.IdCarrinho.ToString(),
            Expires = true,
            ExpirationDateFrom = DateTime.Now,
            ExpirationDateTo = DateTime.Now.AddMinutes(30), 
            Marketplace = "GYMBROS",
            MarketplaceFee = 0
        };

        var client = new PreferenceClient();
        var preference = await client.CreateAsync(request);

        Console.WriteLine($"[MERCADOPAGO] Preferência criada: {preference.Id}");
        Console.WriteLine($"[MERCADOPAGO] URL de pagamento: {preference.InitPoint}");

        return preference.InitPoint;
    }
}