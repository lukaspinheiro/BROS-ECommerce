using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Crud;

namespace BROS_ECommerce.Domain.Interfaces.Repository;

public interface IRepositoryTenant : IDisposable, IRepositoryBase<Tenant>, IAdicionarAsync<Tenant>, IAtualizarAsync<Tenant>
{
    Task<List<Tenant>> ObterTodosAsync();
    Task<bool> TenantExisteAsync(string dominio, string? excluirId = null);
}
