using BROS_ECommerce.Domain.Entities;

namespace BROS_ECommerce.Core.Interfaces;

public interface ITenantAtualService
{
    string? TenantId { get; set; }
    Tenant? TenantAtual { get; }
    public Task<bool> SetTenant(string tenant);
}
