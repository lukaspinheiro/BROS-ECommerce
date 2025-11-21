using BROS_ECommerce.Services.ViewModel.Tenant;

namespace BROS_ECommerce.Services.Interface.Services;

public interface IServiceTenant
{
    Task<List<TabelaTenantViewModel>> ObterTabelaTenantsAsync();
    Task CadastrarTenantAsync(IndexTenantViewModel indexTenantViewModel);
    Task EditarTenantAsync(IndexTenantViewModel indexTenantViewModel);
}
