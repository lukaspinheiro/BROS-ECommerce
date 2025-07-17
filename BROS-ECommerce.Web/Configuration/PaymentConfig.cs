using Microsoft.Extensions.Options;

namespace BROS_ECommerce.Web.Configuration
{
    public class PaymentConfig
    {
        public string BaseUrl { get; set; } = "https://localhost:8081";
        public StripeConfig Stripe { get; set; } = new();
        public MercadoPagoConfig MercadoPago { get; set; } = new();
    }

    public class StripeConfig
    {
        public string SecretKey { get; set; } = string.Empty;
        public string PublishableKey { get; set; } = string.Empty;
    }

    public class MercadoPagoConfig
    {
        public string AccessToken { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
    }

    public static class PaymentConfigExtensions
    {
        public static IServiceCollection ConfigurePaymentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PaymentConfig>(options =>
            {
                options.BaseUrl = configuration["App:BaseUrl"] ?? "https://localhost:8081";
                options.Stripe.SecretKey = configuration["Stripe:SecretKey"] ?? "";
                options.Stripe.PublishableKey = configuration["Stripe:PublishableKey"] ?? "";
                options.MercadoPago.AccessToken = configuration["MercadoPago:AccessToken"] ?? "";
                options.MercadoPago.PublicKey = configuration["MercadoPago:PublicKey"] ?? "";
            });

            return services;
        }
    }
}