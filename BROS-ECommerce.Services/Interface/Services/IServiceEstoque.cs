using BROS_ECommerce.Services.ViewModel.Estoque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServiceEstoque
    {
        Task<List<TabelaEstoqueViewModel>> ObterTabelaEstoqueAsync();

    }
}
