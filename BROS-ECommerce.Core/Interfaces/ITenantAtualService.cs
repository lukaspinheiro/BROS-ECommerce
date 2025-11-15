namespace BROS_ECommerce.Core.Interfaces;

public interface ITenantAtualService
{
    string? TenantId { get; set; }
    public Task<bool> SetTenant(string tenant);
}
