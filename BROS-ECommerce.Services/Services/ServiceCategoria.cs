using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Categoria;
using BROS_ECommerce.Services.ViewModel.CategoriaProduto;
using BROS_ECommerce.Services.ViewModel.Produto;

namespace BROS_ECommerce.Services.Services
{
    public class ServiceCategoria : IServiceCategoria
    {
        private readonly IRepositoryCategoria _repositoryCategoria;

        public ServiceCategoria(IRepositoryCategoria repositoryCategoria)
        {
            _repositoryCategoria = repositoryCategoria;
        }


        public async Task<List<TabelaCategoriaViewModel>> ObterTabelaCategoriaAsync()
        {
            var categorias = await _repositoryCategoria.ObterTodasCategorias();

            return categorias.Select(c => new TabelaCategoriaViewModel(
                idCategoria: c.IdCategoria,
                nomeCategoria: c.NomeCategoria,
                descricao: c.Descricao ?? string.Empty,
                ativo: c.Ativo,
                dataCriacao: c.DataCriacao,
                dataAtualizacao: c.DataAtualizacao ?? DateTime.UtcNow
            )).ToList();
        }
    }
}
