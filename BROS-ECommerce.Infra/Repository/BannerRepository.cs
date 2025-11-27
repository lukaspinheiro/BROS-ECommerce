using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Infra.Repository;


public class BannerRepository : RepositoryBase<Banner>, IRepositoryBanner
{
    public BannerRepository(BrosContext context) : base(context) { }

    public async Task AdicionarAsync(Banner banner)
    {
        banner.DataCriacao = DateTime.UtcNow;
        _dbSet.Add(banner);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Banner banner)
    {
        banner.DataAtualizacao = DateTime.UtcNow;
        _dbSet.Update(banner);
        await _context.SaveChangesAsync();
    }

    public async Task<Banner?> ObterPorIdAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(b => b.IdBanner == id);
    }

    public async Task<List<Banner>> ObterTodosAsync()
    {
        return await _dbSet.OrderByDescending(b => b.DataCriacao).ToListAsync();
    }

    public async Task ExcluirAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ExcluirLogicamenteAsync(Guid id)
    {
        var banner = await _dbSet.FindAsync(id);
        if (banner != null)
        {
            banner.Ativo = false;
            banner.DataAtualizacao = AgoraPortoVelho();
            await _context.SaveChangesAsync();
        }
    }

    private static DateTime AgoraPortoVelho()
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
    }
}
