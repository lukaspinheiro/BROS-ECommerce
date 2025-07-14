using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Domain.Interfaces.Crud
{
    public interface IAdicionarAsync<TEntity> where TEntity : class
    {
        Task AdicionarAsync(TEntity obj);
    }
}
