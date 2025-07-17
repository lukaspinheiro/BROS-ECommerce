using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Services.ViewModel.CategoriaProduto;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServiceCategoriaProduto
    {
        Task<List<CategoriaProdutoViewModel>> ListarPorProdutoAsync(Guid idProduto);
        Task<CategoriaProduto> AdicionarAsync(CategoriaProdutoViewModel viewModel);
        Task RemoverAsync(Guid idCategoriaProduto);
        Task RemoverPorCategoriaAsync(Guid idCategoria);
        Task RemoverPorProdutoAsync(Guid idProduto);

    }

}
