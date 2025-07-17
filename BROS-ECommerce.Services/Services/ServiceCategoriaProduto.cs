using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.CategoriaProduto;

namespace BROS_ECommerce.Services.Services
{
    public class ServiceCategoriaProduto : IServiceCategoriaProduto
    {
        private readonly IRepositoryCategoriaProduto _repositoryCategoriaProduto;

        public ServiceCategoriaProduto(IRepositoryCategoriaProduto repositoryCategoriaProduto)
        {
            _repositoryCategoriaProduto = repositoryCategoriaProduto;
        }

        public async Task<List<CategoriaProdutoViewModel>> ListarPorProdutoAsync(Guid idProduto)
        {
            var lista = await _repositoryCategoriaProduto.ListarPorProdutoAsync(idProduto);

            return lista.Select(cp => new CategoriaProdutoViewModel
            {
                IdCategoriaProduto = cp.IdCategoriaProduto,
                IdCategoria = cp.IdCategoria,
                IdProduto = cp.IdProduto,
                DataAssociacao = cp.DataAssociacao,
                NomeCategoria = cp.Categoria?.NomeCategoria ?? string.Empty,
                NomeProduto = cp.Produto?.Nome ?? string.Empty
            }).ToList();
        }

        public async Task<CategoriaProduto> AdicionarAsync(CategoriaProdutoViewModel viewModel)
        {
            var entidade = new CategoriaProduto
            {
                IdCategoriaProduto = Guid.NewGuid(),
                IdCategoria = viewModel.IdCategoria,
                IdProduto = viewModel.IdProduto,
                DataAssociacao = DateTime.UtcNow
            };

            await _repositoryCategoriaProduto.AdicionarAsync(entidade);
            return entidade;
        }


        public async Task RemoverAsync(Guid idCategoriaProduto)
        {
            await _repositoryCategoriaProduto.ExcluirAsync(idCategoriaProduto);
        }
    }

}
