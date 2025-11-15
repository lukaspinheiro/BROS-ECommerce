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

        await tenantAtualService.SetTenant(tenant);

        await _next(context);
    }


}
