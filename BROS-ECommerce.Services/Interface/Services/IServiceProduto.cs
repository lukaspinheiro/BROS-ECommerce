using BROS_ECommerce.Services.ViewModel.Produto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServiceProduto
    {
        List<ProdutoViewModel> ObterTodos();
        ProdutoViewModel? ObterPorSlug(string slug);
        Task Adicionar(CadastrarProdutoViewModel produtoVM);

    }
}
