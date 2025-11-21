using BROS_ECommerce.Core.Interfaces;
using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Services.Services;

public  class TenantAtualService : ITenantAtualService
{
    private readonly TenantDbContext _context;

    public TenantAtualService (TenantDbContext context)
    {
        _context = context;
    }

    public string? TenantId { get; set; }
    public Tenant? TenantAtual { get; private set; }


    public async Task<bool> SetTenant(string tenant)
    {
        var tenantInfo = await _context.Tenants.Where(x => x.Id == tenant).FirstOrDefaultAsync();
        
        if (tenantInfo == null)
            return false;

        if (!tenantInfo.Ativo)
            return false;

        TenantId = tenantInfo.Id;
        TenantAtual = tenantInfo;
        return true;
    }
}
