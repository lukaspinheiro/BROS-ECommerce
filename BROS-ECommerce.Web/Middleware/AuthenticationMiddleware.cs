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
            try
            {
                var path = context.Request.Path.Value?.ToLower() ?? "";
                var method = context.Request.Method;

                var requiresAuth = new[]
                {
                    "/carrinho/checkout",
                    "/carrinho/finalizarcompra",
                    "/carrinho/irparacarrinho",
                    "/meuspedidos"
                };

                if (method == "GET" && RequiresAuthentication(path, requiresAuth))
                {
                    if (!context.User.Identity?.IsAuthenticated == true)
                    {
                        context.Response.Redirect("/Autenticacao/Login?returnUrl=" +
                                                System.Net.WebUtility.UrlEncode(context.Request.Path + context.Request.QueryString));
                        return;
                    }
                }

                if (method == "POST" && (path.Contains("/carrinho/") || path.Contains("/checkout/") || path.Contains("/meuspedidos/")))
                {
                    if (!context.User.Identity?.IsAuthenticated == true)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("{\"sucesso\":false,\"mensagem\":\"Usuário não autenticado\",\"redirectToLogin\":true}");
                        return;
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Erro no AuthenticationMiddleware: {ex.Message}");
                await _next(context);
            }
        }

        private static bool RequiresAuthentication(string path, string[] requiresAuth)
        {
            foreach (var route in requiresAuth)
            {
                if (path.StartsWith(route))
                {
                    return true;
                }
            }
            return false;
        }
    }
}