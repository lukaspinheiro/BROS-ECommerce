using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Carrinho;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.Services
{
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly IConfiguration _config;
        private readonly string _accessToken;

        public MercadoPagoService(IConfiguration config)
        {
            _config = config;
            _accessToken = config["MercadoPago:AccessToken"];
            MercadoPagoConfig.AccessToken = _accessToken;
        }

        public async Task<string> CriarPreferenciaAsync(CarrinhoViewModel carrinho)
        {
            try
            {
                var baseUrl = _config["App:BaseUrl"] ?? "https://localhost:8081";

                Console.WriteLine($"[MERCADOPAGO] Iniciando criação de preferência");
                Console.WriteLine($"[MERCADOPAGO] AccessToken: {_accessToken.Substring(0, 20)}...");
                Console.WriteLine($"[MERCADOPAGO] BaseUrl: {baseUrl}");
                Console.WriteLine($"[MERCADOPAGO] Carrinho ID: {carrinho.IdCarrinho}");
                Console.WriteLine($"[MERCADOPAGO] Total de itens: {carrinho.Itens.Count}");

                // Criar lista de itens
                var items = carrinho.Itens.Select(item => new PreferenceItemRequest
                {
                    Id = item.IdProduto.ToString(),
                    Title = item.Nome,
                    Description = $"Produto: {item.Nome}",
                    Quantity = item.Quantidade,
                    CurrencyId = "BRL",
                    UnitPrice = item.PrecoUnitario,
                    PictureUrl = !string.IsNullOrEmpty(item.ImagemUrl) && !item.ImagemUrl.StartsWith("/")
                        ? item.ImagemUrl
                        : $"{baseUrl}{item.ImagemUrl}",
                    CategoryId = "health"
                }).ToList();

                // Log dos itens
                foreach (var item in items)
                {
                    Console.WriteLine($"[MERCADOPAGO] Item: {item.Title} - Qtd: {item.Quantity} - Preço: R$ {item.UnitPrice:F2}");
                }

                // Criar preferência com URLs corretas
                var request = new PreferenceRequest
                {
                    Items = items,
                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        Success = $"{baseUrl}/pagamento/sucesso",
                        Failure = $"{baseUrl}/pagamento/cancelado",
                        Pending = $"{baseUrl}/pagamento/cancelado"
                    },
                    AutoReturn = "approved",
                    PaymentMethods = new PreferencePaymentMethodsRequest
                    {
                        ExcludedPaymentMethods = new List<PreferencePaymentMethodRequest>(),
                        ExcludedPaymentTypes = new List<PreferencePaymentTypeRequest>(),
                        Installments = 12
                    },
                    NotificationUrl = $"{baseUrl}/pagamento/webhook/mercadopago",
                    StatementDescriptor = "GYMBROS SUPPLEMENTS",
                    ExternalReference = carrinho.IdCarrinho.ToString(),
                    Expires = true,
                    ExpirationDateFrom = DateTime.Now,
                    ExpirationDateTo = DateTime.Now.AddMinutes(30)
                };

                Console.WriteLine($"[MERCADOPAGO] Success URL: {request.BackUrls.Success}");
                Console.WriteLine($"[MERCADOPAGO] Failure URL: {request.BackUrls.Failure}");
                Console.WriteLine($"[MERCADOPAGO] Notification URL: {request.NotificationUrl}");

                var client = new PreferenceClient();
                Preference preference = await client.CreateAsync(request);

                Console.WriteLine($"[MERCADOPAGO] ✅ Preferência criada com sucesso!");
                Console.WriteLine($"[MERCADOPAGO] ID: {preference.Id}");
                Console.WriteLine($"[MERCADOPAGO] InitPoint: {preference.InitPoint}");
                Console.WriteLine($"[MERCADOPAGO] SandboxInitPoint: {preference.SandboxInitPoint}");

                // Retornar URL correta baseado no ambiente (TEST- = sandbox)
                var checkoutUrl = _accessToken.StartsWith("TEST-")
                    ? preference.SandboxInitPoint
                    : preference.InitPoint;

                Console.WriteLine($"[MERCADOPAGO] URL de checkout: {checkoutUrl}");

                return checkoutUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MERCADOPAGO] ❌ ERRO ao criar preferência:");
                Console.WriteLine($"[MERCADOPAGO] Mensagem: {ex.Message}");
                Console.WriteLine($"[MERCADOPAGO] StackTrace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[MERCADOPAGO] Inner Exception: {ex.InnerException.Message}");
                }

                throw new Exception($"Erro ao criar preferência no Mercado Pago: {ex.Message}", ex);
            }
        }
    }
}
