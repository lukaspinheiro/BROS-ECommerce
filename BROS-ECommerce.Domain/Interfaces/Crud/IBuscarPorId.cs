using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Domain.Interfaces.Crud
{
    public interface IBuscarPorId<TEntity> where TEntity : class
    {
        TEntity BuscarPorId(Guid id, bool somenteLeitura = false);
    }
}
