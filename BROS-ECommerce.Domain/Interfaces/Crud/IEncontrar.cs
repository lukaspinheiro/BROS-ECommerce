using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Domain.Interfaces.Crud
{
    public interface IEncontrar<TEntity> where TEntity : class
    {
        List<TEntity> Encontrar(Expression<Func<TEntity, bool>> expressao);
    }
}
