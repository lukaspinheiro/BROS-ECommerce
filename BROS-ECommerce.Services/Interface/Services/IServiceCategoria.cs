using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Services.ViewModel.Categoria;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServiceCategoria
    {
        Task<List<TabelaCategoriaViewModel>> ObterTabelaCategoriaAsync();
        Task AdicionarCategoriaAsync(CadastrarCategoriaViewModel CategoriaVM);
        Task ExcluirAsync(Guid id);
        Task EditarCategoriaAsync(CadastrarCategoriaViewModel model);
        Task<List<Categoria>> ListarTodasAsync();
        Task<CategoriaViewModel?> ObterPorIdAsync(Guid idCategoria);

    }
}
