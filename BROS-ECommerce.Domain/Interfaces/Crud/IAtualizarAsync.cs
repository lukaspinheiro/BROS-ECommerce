
using System.Threading.Tasks;

namespace BROS_ECommerce.Domain.Interfaces.Crud
{
    public interface IAtualizarAsync<TEntity> where TEntity : class
    {
        Task AtualizarAsync(TEntity obj);
    }
}
