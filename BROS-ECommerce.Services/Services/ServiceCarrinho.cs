using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Carrinho;

namespace BROS_ECommerce.Services.Services
{
    public class ServiceCarrinho : IServiceCarrinho
    {
        private readonly IRepositoryCarrinho _repositoryCarrinho;
        private readonly IRepositoryCarrinhoItem _repositoryCarrinhoItem;
        private readonly IRepositoryProduto _repositoryProduto;

        public ServiceCarrinho(
            IRepositoryCarrinho repositoryCarrinho,
            IRepositoryCarrinhoItem repositoryCarrinhoItem,
            IRepositoryProduto repositoryProduto)
        {
            _repositoryCarrinho = repositoryCarrinho;
            _repositoryCarrinhoItem = repositoryCarrinhoItem;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<CarrinhoViewModel?> ObterCarrinhoUsuarioAsync(Guid idUsuario)
        {
            var carrinho = await _repositoryCarrinho.ObterCarrinhoAbertoUsuarioAsync(idUsuario);
            if (carrinho == null) return null;

            return await MapearCarrinhoParaViewModel(carrinho);
        }

        public async Task<CarrinhoViewModel?> ObterCarrinhoPorIdAsync(Guid idCarrinho)
        {
            var carrinho = await _repositoryCarrinho.ObterCarrinhoComItensAsync(idCarrinho);
            if (carrinho == null) return null;

            return await MapearCarrinhoParaViewModel(carrinho);
        }

        public async Task<CarrinhoViewModel> CriarCarrinhoAsync(Guid? idUsuario = null)
        {
            var carrinho = new Carrinho
            {
                IdCarrinho = Guid.NewGuid(),
                IdUsuario = idUsuario,
                DataCriacao = DateTime.UtcNow,
                Status = "Aberto"
            };

            await _repositoryCarrinho.CriarCarrinhoAsync(carrinho);
            return await MapearCarrinhoParaViewModel(carrinho);
        }

        public async Task<CarrinhoViewModel> AdicionarProdutoAsync(Guid? idUsuario, Guid idProduto, int quantidade = 1)
        {
            var carrinho = idUsuario.HasValue
                ? await _repositoryCarrinho.ObterCarrinhoAbertoUsuarioAsync(idUsuario.Value)
                : null;

            if (carrinho == null)
            {
                carrinho = new Carrinho
                {
                    IdCarrinho = Guid.NewGuid(),
                    IdUsuario = idUsuario,
                    DataCriacao = DateTime.UtcNow,
                    Status = "Aberto"
                };
                await _repositoryCarrinho.CriarCarrinhoAsync(carrinho);
            }

            var produto = await _repositoryProduto.ObterPorIdAsync(idProduto);
            if (produto == null)
                throw new ArgumentException("Produto não encontrado");

            var itemExistente = await _repositoryCarrinhoItem.ObterPorCarrinhoEProdutoAsync(carrinho.IdCarrinho, idProduto);

            if (itemExistente != null)
            {
                itemExistente.Quantidade += quantidade;
                await _repositoryCarrinhoItem.AtualizarItemAsync(itemExistente);
            }
            else
            {
                var novoItem = new CarrinhoItem
                {
                    IdCarrinhoItem = Guid.NewGuid(),
                    IdCarrinho = carrinho.IdCarrinho,
                    IdProduto = idProduto,
                    Quantidade = quantidade,
                    PrecoUnitario = produto.Preco
                };
                await _repositoryCarrinhoItem.AdicionarItemAsync(novoItem);
            }

            return await ObterCarrinhoPorIdAsync(carrinho.IdCarrinho) ?? new CarrinhoViewModel();
        }

        public async Task<CarrinhoViewModel> AtualizarQuantidadeAsync(Guid idCarrinho, Guid idProduto, int quantidade)
        {
            var item = await _repositoryCarrinhoItem.ObterPorCarrinhoEProdutoAsync(idCarrinho, idProduto);
            if (item == null)
                throw new ArgumentException("Item não encontrado no carrinho");

            if (quantidade <= 0)
            {
                await _repositoryCarrinhoItem.RemoverItemAsync(item.IdCarrinhoItem);
            }
            else
            {
                item.Quantidade = quantidade;
                await _repositoryCarrinhoItem.AtualizarItemAsync(item);
            }

            return await ObterCarrinhoPorIdAsync(idCarrinho) ?? new CarrinhoViewModel();
        }

        public async Task<bool> RemoverProdutoAsync(Guid idCarrinho, Guid idProduto)
        {
            var item = await _repositoryCarrinhoItem.ObterPorCarrinhoEProdutoAsync(idCarrinho, idProduto);
            if (item == null) return false;

            return await _repositoryCarrinhoItem.RemoverItemAsync(item.IdCarrinhoItem);
        }

        public async Task<bool> LimparCarrinhoAsync(Guid idCarrinho)
        {
            return await _repositoryCarrinhoItem.RemoverTodosItensCarrinhoAsync(idCarrinho);
        }

        public async Task<bool> FinalizarCarrinhoAsync(Guid idCarrinho)
        {
            return await _repositoryCarrinho.FinalizarCarrinhoAsync(idCarrinho);
        }

        public async Task<int> ObterQuantidadeItensAsync(Guid? idUsuario)
        {
            if (!idUsuario.HasValue) return 0;

            var carrinho = await _repositoryCarrinho.ObterCarrinhoAbertoUsuarioAsync(idUsuario.Value);
            if (carrinho == null) return 0;

            return await _repositoryCarrinhoItem.ContarItensCarrinhoAsync(carrinho.IdCarrinho);
        }

        public async Task<decimal> ObterTotalCarrinhoAsync(Guid idCarrinho)
        {
            return await _repositoryCarrinhoItem.CalcularTotalCarrinhoAsync(idCarrinho);
        }

        private async Task<CarrinhoViewModel> MapearCarrinhoParaViewModel(Carrinho carrinho)
        {
            var itens = await _repositoryCarrinhoItem.ObterItensPorCarrinhoAsync(carrinho.IdCarrinho);

            return new CarrinhoViewModel
            {
                IdCarrinho = carrinho.IdCarrinho,
                IdUsuario = carrinho.IdUsuario,
                DataCriacao = carrinho.DataCriacao,
                Status = carrinho.Status,
                Itens = itens.Select(i => new ItemCarrinhoViewModel
                {
                    IdCarrinhoItem = i.IdCarrinhoItem,
                    IdCarrinho = i.IdCarrinho,
                    IdProduto = i.IdProduto,
                    Nome = i.Produto?.Nome ?? "Produto não encontrado",
                    Slug = i.Produto?.Slug ?? "",
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    ImagemUrl = i.Produto?.ProdutoImagens?.FirstOrDefault()?.Imagem?.CaminhoArquivo ?? "/img/produto-placeholder.jpg"
                }).ToList()
            };
        }
    }
}