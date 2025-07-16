using BROS_ECommerce.Services.ViewModel.Categoria;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServiceCategoria
    {
        Task<List<TabelaCategoriaViewModel>> ObterTabelaCategoriaAsync();
    }
}
