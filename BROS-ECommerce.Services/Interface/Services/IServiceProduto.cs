using BROS_ECommerce.Services.ViewModel.Produto;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServiceProduto
    {
        List<ProdutoViewModel> ObterTodos();
        ProdutoViewModel? ObterPorSlug(string slug);

    }
}
