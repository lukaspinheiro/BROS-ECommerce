using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Carrinho;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Configuration;

public class MercadoPagoService : IMercadoPagoService
{
    public MercadoPagoService(IConfiguration config)
    {
        MercadoPagoConfig.AccessToken = config["MercadoPago:AccessToken"];
    }

    public async Task<string> CriarPreferenciaAsync(CarrinhoViewModel carrinho)
    {
        var items = carrinho.Itens.Select(item => new PreferenceItemRequest
        {
            Title = item.Nome,
            Quantity = item.Quantidade,
            CurrencyId = "BRL",
            UnitPrice = item.PrecoUnitario
        }).ToList();

        var request = new PreferenceRequest
        {
            Items = items,
            BackUrls = new PreferenceBackUrlsRequest
            {
                Success = "https://seusite.com/pedido/sucesso",
                Failure = "https://seusite.com/carrinho",
                Pending = "https://seusite.com/pedido/pendente"
            },
            AutoReturn = "approved"
        };

        var client = new PreferenceClient();
        var preference = await client.CreateAsync(request);
        return preference.InitPoint;
    }
}
