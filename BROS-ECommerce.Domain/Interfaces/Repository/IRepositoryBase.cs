using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class, IAggregateRoot
    {
        void Adicionar(TEntity entity);
        void Atualizar(TEntity entity);
        void Remover(dynamic id);
        Task<TEntity> BuscarPorId(dynamic Id);
        Task<IEnumerable<TEntity>> BuscarTodos();
    }
}
