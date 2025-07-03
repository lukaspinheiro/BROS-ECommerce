using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Estoque;

namespace BROS_ECommerce.Services.Services
{
    public class ServiceEstoque : IServiceEstoque
    {
        private readonly IRepositoryEstoque _repositoryEstoque;
        public ServiceEstoque(IRepositoryEstoque repositoryEstoque)
        {
            _repositoryEstoque = repositoryEstoque;
        }

        public async Task<List<TabelaEstoqueViewModel>> ObterTabelaEstoqueAsync()
        {
            var estoque = await _repositoryEstoque.ObterTodosAsync();

            return estoque.Select(e => new TabelaEstoqueViewModel
            {
                IdEstoque = e.IdEstoque,
                IdProduto = e.IdProduto,
                Nome = e.Nome,
                UltimaAtualizacao = e.UltimaAtualizacao,
                Quantidade = e.Quantidade
            }).ToList();
        }
    }
}
