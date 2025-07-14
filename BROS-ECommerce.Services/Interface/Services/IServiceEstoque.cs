using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Services.ViewModel.Estoque;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServiceEstoque
    {
        Task<List<Estoque>> ObterTodosAsync();
        Task<Estoque?> ObterPorIdAsync(Guid id);
        Task<Estoque?> ObterPorIdProdutoAsync(Guid idProduto);
        Task<List<Estoque>> ObterEstoquesBaixosAsync(int quantidadeMinima = 5);
        Task<List<Estoque>> ObterEstoquesComProdutosAsync();

        Task CriarEstoqueParaProdutoAsync(Guid idProduto, int quantidadeInicial = 1);
        Task AtualizarQuantidadeAsync(Guid idProduto, int novaQuantidade, DateTime UltimaAtualizacao);
        Task AdicionarQuantidadeAsync(Guid idProduto, int quantidadeAdicionar, DateTime UltimaAtualizacao);
        Task RemoverQuantidadeAsync(Guid idProduto, int quantidadeRemover);
        Task ExcluirAsync(Guid id);
        Task ExcluirPorIdProdutoAsync(Guid idProduto);

        Task<bool> VerificarDisponibilidadeAsync(Guid idProduto, int quantidadeSolicitada);
        Task<bool> ExisteEstoqueParaProdutoAsync(Guid idProduto);
        Task PopularEstoqueInicialAsync();
        Task<List<TabelaEstoqueViewModel>> ObterTabelaEstoqueAsync();
        Task AdicionarProdutoNoEstoqueAsync(CadastrarEstoqueViewModel cadastrarEstoqueViewModel);
    }
}