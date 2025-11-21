using BROS_ECommerce.Core.Interfaces;

namespace BROS_ECommerce.Web.Middleware;

public class TenantResolver
{
    private readonly RequestDelegate _next;

    public TenantResolver(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantAtualService tenantAtualService)
    {
        var host = context.Request.Host.Host;
        var partes = host.Split('.');

        string tenant = partes.Length >= 3 ? partes[0] : "default";

        var reservados = new[] { "localhost", "www", "api" };

        if (reservados.Contains(tenant.ToLower()))
            tenant = "default";

        var valido = await tenantAtualService.SetTenant(tenant);

        if (!valido)
        {
            var pathAndQuery = context.Request.Path + context.Request.QueryString;
            var redirectUrl = $"https://ecommerce.bros.localhost:8081{pathAndQuery}";

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status302Found;
            context.Response.Headers["Location"] = redirectUrl;
            context.Response.Headers["Connection"] = "close";
            context.Response.ContentLength = 0;

            await context.Response.CompleteAsync();
            return;

        }
        await _next(context);
    }
}
