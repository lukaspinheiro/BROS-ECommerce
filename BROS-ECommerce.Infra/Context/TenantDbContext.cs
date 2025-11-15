using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Infra.Context;

public class TenantDbContext : DbContext
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
    {
    }

    public DbSet<Tenant> Tenants { get; set; }
}
