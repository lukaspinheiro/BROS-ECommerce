using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BROS_ECommerce.Web.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            var method = context.Request.Method;

            var requiresAuth = new[]
            {
                "/carrinho/checkout",
                "/carrinho/finalizarcompra",
                "/carrinho/irparacarrinho"
            };

            if (method == "GET" && requiresAuth.Any(route => path?.StartsWith(route) == true))
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    context.Response.Redirect("/Autenticacao/Login?returnUrl=" +
                                            System.Net.WebUtility.UrlEncode(context.Request.Path + context.Request.QueryString));
                    return;
                }
            }

            if (method == "POST" && (path?.Contains("/carrinho/") == true || path?.Contains("/checkout/") == true))
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("{\"sucesso\":false,\"mensagem\":\"Usuário não autenticado\",\"redirectToLogin\":true}");
                    return;
                }
            }

            await _next(context);
        }
    }
}