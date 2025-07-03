using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;

namespace BROS_ECommerce.Services.Services
{
    public class EstoqueService : IServiceEstoque
    {
        private readonly IRepositoryEstoque _repositoryEstoque;
        private readonly IRepositoryProduto _repositoryProduto;

        public EstoqueService(IRepositoryEstoque repositoryEstoque, IRepositoryProduto repositoryProduto)
        {
            _repositoryEstoque = repositoryEstoque;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<List<Estoque>> ObterTodosAsync()
        {
            return await _repositoryEstoque.ObterEstoquesComProdutosAsync();
        }

        public async Task<Estoque?> ObterPorIdAsync(Guid id)
        {
            return await _repositoryEstoque.ObterPorIdAsync(id);
        }

        public async Task<Estoque?> ObterPorIdProdutoAsync(Guid idProduto)
        {
            return await _repositoryEstoque.ObterPorIdProdutoAsync(idProduto);
        }

        public async Task<List<Estoque>> ObterEstoquesBaixosAsync(int quantidadeMinima = 5)
        {
            return await _repositoryEstoque.ObterEstoquesBaixosAsync(quantidadeMinima);
        }

        public async Task<List<Estoque>> ObterEstoquesComProdutosAsync()
        {
            return await _repositoryEstoque.ObterEstoquesComProdutosAsync();
        }

        public async Task CriarEstoqueParaProdutoAsync(Guid idProduto, int quantidadeInicial = 1)
        {
            var produto = await _repositoryProduto.ObterPorIdAsync(idProduto);
            if (produto == null)
            {
                throw new InvalidOperationException("Produto não encontrado");
            }

            var estoqueExistente = await _repositoryEstoque.ExistePorIdProdutoAsync(idProduto);
            if (estoqueExistente)
            {
                throw new InvalidOperationException($"Já existe estoque para o produto '{produto.Nome}'");
            }

            var estoque = new Estoque(idProduto, quantidadeInicial);
            await _repositoryEstoque.AdicionarAsync(estoque);
        }

        public async Task AtualizarQuantidadeAsync(Guid idProduto, int novaQuantidade)
        {
            if (novaQuantidade < 0)
            {
                throw new ArgumentException("A quantidade não pode ser negativa");
            }

            await _repositoryEstoque.AtualizarQuantidadeAsync(idProduto, novaQuantidade);
        }

        public async Task AdicionarQuantidadeAsync(Guid idProduto, int quantidadeAdicionar)
        {
            if (quantidadeAdicionar <= 0)
            {
                throw new ArgumentException("A quantidade a adicionar deve ser maior que zero");
            }

            await _repositoryEstoque.AdicionarQuantidadeAsync(idProduto, quantidadeAdicionar);
        }

        public async Task RemoverQuantidadeAsync(Guid idProduto, int quantidadeRemover)
        {
            if (quantidadeRemover <= 0)
            {
                throw new ArgumentException("A quantidade a remover deve ser maior que zero");
            }

            await _repositoryEstoque.RemoverQuantidadeAsync(idProduto, quantidadeRemover);
        }

        public async Task ExcluirAsync(Guid id)
        {
            await _repositoryEstoque.ExcluirAsync(id);
        }

        public async Task ExcluirPorIdProdutoAsync(Guid idProduto)
        {
            await _repositoryEstoque.ExcluirPorIdProdutoAsync(idProduto);
        }

        public async Task<bool> VerificarDisponibilidadeAsync(Guid idProduto, int quantidadeSolicitada)
        {
            return await _repositoryEstoque.VerificarDisponibilidadeAsync(idProduto, quantidadeSolicitada);
        }

        public async Task<bool> ExisteEstoqueParaProdutoAsync(Guid idProduto)
        {
            return await _repositoryEstoque.ExistePorIdProdutoAsync(idProduto);
        }

        public async Task PopularEstoqueInicialAsync()
        {
            var produtos = await _repositoryProduto.ObterTodosAsync();

            foreach (var produto in produtos)
            {
                var jaExisteEstoque = await _repositoryEstoque.ExistePorIdProdutoAsync(produto.IdProduto);

                if (!jaExisteEstoque)
                {
                    var estoque = new Estoque(produto.IdProduto, 1);
                    await _repositoryEstoque.AdicionarAsync(estoque);

                    Console.WriteLine($"Estoque criado para produto: {produto.Nome} (Quantidade: 1)");
                }
            }
        }
    }
}