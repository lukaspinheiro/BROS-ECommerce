using BROS_ECommerce.Domain.Interfaces;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Infra.Repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IAggregateRoot
    {
        protected readonly BrosContext _context;
        protected DbSet<TEntity> _dbSet;

        public RepositoryBase(BrosContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void Adicionar(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Atualizar(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<TEntity> BuscarPorId(dynamic Id)
        {
            var result = await _dbSet.FindAsync(Id);
            return result;
        }

        public async Task<IEnumerable<TEntity>> BuscarTodos()
        {
            return await _dbSet.ToListAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Remover(dynamic id)
        {
            _dbSet.Remove(_dbSet.Find(id));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

    }
}
