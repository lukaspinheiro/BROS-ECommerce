using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Domain.Interfaces.Crud
{
    public interface IRemover<TEntity> where TEntity : class
    {
        void Remover(Guid id);
    }
}
