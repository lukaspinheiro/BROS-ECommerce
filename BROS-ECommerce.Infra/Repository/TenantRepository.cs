using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Infra.Repository;

public class TenantRepository : RepositoryBase<Tenant>, IRepositoryTenant
{
    public TenantRepository(BrosContext context) : base(context){ }

    public async Task<List<Tenant>> ObterTodosAsync()
    {
        return await _dbSet.OrderByDescending(t => t.DataCriacao).ToListAsync();
    }
    public async Task<bool> TenantExisteAsync(string dominio, string? excluirId = null)
    {
        return await _dbSet.AnyAsync(p =>
            p.Id == dominio &&
            (excluirId == null || p.Id != excluirId)
        );
    }

    public async Task AdicionarAsync(Tenant tenant)
    {
        _dbSet.Add(tenant);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Tenant tenant)
    {
        _dbSet.Update(tenant);
        await _context.SaveChangesAsync();
    }

}
