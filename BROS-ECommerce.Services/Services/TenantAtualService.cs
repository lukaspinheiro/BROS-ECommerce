using BROS_ECommerce.Core.Interfaces;
using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Services.Services;

public  class TenantAtualService : ITenantAtualService
{
    private readonly TenantDbContext _contextTenant;
    private readonly ImagemDbContext _contextImagem;

    public TenantAtualService (TenantDbContext contextTenant, ImagemDbContext contextImagem)
    {
        _contextTenant = contextTenant;
        _contextImagem = contextImagem;
    }

    public string? TenantId { get; set; }
    public Tenant? TenantAtual { get; private set; }


    public async Task<bool> SetTenant(string tenant)
    {
        //var tenantInfo = await _context.Tenants.Where(x => x.Id == tenant).FirstOrDefaultAsync();
        var tenantInfo = await _contextTenant.Tenants.FirstOrDefaultAsync(x => x.Id == tenant);

        if (tenantInfo == null || !tenantInfo.Ativo)
            return false;

        TenantId = tenantInfo.Id;
        TenantAtual = tenantInfo;

        if (tenantInfo.IdLogo.HasValue)
        {
            tenantInfo.Logo = await _contextImagem.Imagens
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.IdImagem == tenantInfo.IdLogo);
        }

        return true;
    }

}
