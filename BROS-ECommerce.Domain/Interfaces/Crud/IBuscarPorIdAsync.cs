using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Domain.Interfaces.Crud
{
    public interface IBuscarPorIdAsync<TEntity> where TEntity : class
    {
        Task<TEntity> BuscarPorIdAsync(Guid id, bool somenteLeitura = false);
    }
}
