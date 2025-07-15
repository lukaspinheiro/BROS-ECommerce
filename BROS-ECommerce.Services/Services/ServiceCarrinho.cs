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
        private readonly IRepositoryEstoque _repositoryEstoque;

        public ServiceCarrinho(
            IRepositoryCarrinho repositoryCarrinho,
            IRepositoryCarrinhoItem repositoryCarrinhoItem,
            IRepositoryProduto repositoryProduto,
            IRepositoryEstoque repositoryEstoque)
        {
            _repositoryCarrinho = repositoryCarrinho;
            _repositoryCarrinhoItem = repositoryCarrinhoItem;
            _repositoryProduto = repositoryProduto;
            _repositoryEstoque = repositoryEstoque;
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
            Console.WriteLine($"[SERVICE] =================================");
            Console.WriteLine($"[SERVICE] ServiceCarrinho.AdicionarProdutoAsync");
            Console.WriteLine($"[SERVICE] Produto ID: {idProduto}");
            Console.WriteLine($"[SERVICE] Usuario ID: {idUsuario}");
            Console.WriteLine($"[SERVICE] Quantidade: {quantidade}");

            var carrinho = idUsuario.HasValue
                ? await _repositoryCarrinho.ObterCarrinhoAbertoUsuarioAsync(idUsuario.Value)
                : null;

            Console.WriteLine($"[SERVICE] Carrinho existente: {carrinho?.IdCarrinho}");

            if (carrinho == null)
            {
                Console.WriteLine($"[SERVICE] Criando novo carrinho...");
                carrinho = new Carrinho
                {
                    IdCarrinho = Guid.NewGuid(),
                    IdUsuario = idUsuario,
                    DataCriacao = DateTime.UtcNow,
                    Status = "Aberto"
                };
                await _repositoryCarrinho.CriarCarrinhoAsync(carrinho);
                Console.WriteLine($"[SERVICE] Novo carrinho criado: {carrinho.IdCarrinho}");
            }

            Console.WriteLine($"[SERVICE] Buscando produto no banco...");

            
            var produto = await _repositoryProduto.ObterPorIdAsync(idProduto);

            Console.WriteLine($"[SERVICE] Produto encontrado: {produto != null}");

            if (produto != null)
            {
                Console.WriteLine($"[SERVICE] Produto - ID: {produto.IdProduto}");
                Console.WriteLine($"[SERVICE] Produto - Nome: {produto.Nome}");
                Console.WriteLine($"[SERVICE] Produto - Preço: {produto.Preco}");
            }

            if (produto == null)
            {
                Console.WriteLine($"[SERVICE] ERRO: Produto não encontrado!");
                Console.WriteLine($"[SERVICE] Produto ID buscado: {idProduto}");
                throw new ArgumentException("Produto não encontrado");
            }

            Console.WriteLine($"[SERVICE] Verificando item existente no carrinho...");
            var itemExistente = await _repositoryCarrinhoItem.ObterPorCarrinhoEProdutoAsync(carrinho.IdCarrinho, idProduto);
            Console.WriteLine($"[SERVICE] Item existente: {itemExistente != null}");

            if (itemExistente != null)
            {
                Console.WriteLine($"[SERVICE] Atualizando quantidade do item existente...");
                Console.WriteLine($"[SERVICE] Quantidade antiga: {itemExistente.Quantidade}");
                itemExistente.Quantidade += quantidade;
                Console.WriteLine($"[SERVICE] Nova quantidade: {itemExistente.Quantidade}");
                await _repositoryCarrinhoItem.AtualizarItemAsync(itemExistente);
            }
            else
            {
                Console.WriteLine($"[SERVICE] Criando novo item no carrinho...");
                var novoItem = new CarrinhoItem
                {
                    IdCarrinhoItem = Guid.NewGuid(),
                    IdCarrinho = carrinho.IdCarrinho,
                    IdProduto = idProduto,
                    Quantidade = quantidade,
                    PrecoUnitario = produto.Preco
                };

                Console.WriteLine($"[SERVICE] Novo item - ID: {novoItem.IdCarrinhoItem}");
                Console.WriteLine($"[SERVICE] Novo item - Carrinho: {novoItem.IdCarrinho}");
                Console.WriteLine($"[SERVICE] Novo item - Produto: {novoItem.IdProduto}");
                Console.WriteLine($"[SERVICE] Novo item - Quantidade: {novoItem.Quantidade}");
                Console.WriteLine($"[SERVICE] Novo item - Preço: {novoItem.PrecoUnitario}");

                await _repositoryCarrinhoItem.AdicionarItemAsync(novoItem);
                Console.WriteLine($"[SERVICE] Item adicionado com sucesso!");
            }

            Console.WriteLine($"[SERVICE] Obtendo carrinho atualizado...");
            var carrinhoAtualizado = await ObterCarrinhoPorIdAsync(carrinho.IdCarrinho) ?? new CarrinhoViewModel();
            Console.WriteLine($"[SERVICE] Carrinho atualizado - Total itens: {carrinhoAtualizado.QuantidadeTotal}");
            Console.WriteLine($"[SERVICE] =================================");

            return carrinhoAtualizado;
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


        public async Task<CarrinhoViewModel> AtualizarQuantidadeProdutoAsync(Guid idUsuario, Guid idProduto, int novaQuantidade)
        {
            if (novaQuantidade <= 0)
            {
                throw new ArgumentException("Quantidade deve ser maior que zero");
            }

            var carrinho = await _repositoryCarrinho.ObterCarrinhoAbertoUsuarioAsync(idUsuario);
            if (carrinho == null)
            {
                throw new InvalidOperationException("Carrinho não encontrado");
            }

            var item = await _repositoryCarrinhoItem.ObterPorCarrinhoEProdutoAsync(carrinho.IdCarrinho, idProduto);
            if (item == null)
            {
                throw new InvalidOperationException("Produto não encontrado no carrinho");
            }

            var produto = await _repositoryProduto.ObterPorIdAsync(idProduto);
            if (produto == null)
            {
                throw new InvalidOperationException("Produto não encontrado");
            }

            var estoque = await _repositoryEstoque.ObterPorIdProdutoAsync(idProduto);
            if (estoque != null && estoque.Quantidade < novaQuantidade)
            {
                throw new InvalidOperationException($"Estoque insuficiente. Disponível: {estoque.Quantidade}");
            }

            item.Quantidade = novaQuantidade;
            await _repositoryCarrinhoItem.AtualizarItemAsync(item);

            return await ObterCarrinhoUsuarioAsync(idUsuario) ?? new CarrinhoViewModel();
        }

        public async Task<CarrinhoViewModel> AdicionarOuAtualizarProdutoAsync(Guid? idUsuario, Guid idProduto, int quantidade)
        {
            if (!idUsuario.HasValue)
            {
                throw new UnauthorizedAccessException("Usuário não autenticado");
            }

            var produto = await _repositoryProduto.ObterPorIdAsync(idProduto);
            if (produto == null)
            {
                throw new InvalidOperationException("Produto não encontrado");
            }

            var estoque = await _repositoryEstoque.ObterPorIdProdutoAsync(idProduto);
            if (estoque != null && estoque.Quantidade < quantidade)
            {
                throw new InvalidOperationException($"Estoque insuficiente. Disponível: {estoque.Quantidade}");
            }

            var carrinho = await _repositoryCarrinho.ObterCarrinhoAbertoUsuarioAsync(idUsuario.Value);
            if (carrinho == null)
            {
                carrinho = new Carrinho
                {
                    IdCarrinho = Guid.NewGuid(),
                    IdUsuario = idUsuario.Value,
                    DataCriacao = DateTime.UtcNow,
                    Status = "Aberto"
                };
                carrinho = await _repositoryCarrinho.CriarCarrinhoAsync(carrinho);
            }

            
            var itemExistente = await _repositoryCarrinhoItem.ObterPorCarrinhoEProdutoAsync(carrinho.IdCarrinho, idProduto);

            if (itemExistente != null)
            {
                
                var novaQuantidade = itemExistente.Quantidade + quantidade;

                if (estoque != null && estoque.Quantidade < novaQuantidade)
                {
                    throw new InvalidOperationException($"Estoque insuficiente. Disponível: {estoque.Quantidade}");
                }

                itemExistente.Quantidade = novaQuantidade;
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

            return await ObterCarrinhoUsuarioAsync(idUsuario.Value) ?? new CarrinhoViewModel();
        }

        public async Task<int> ObterQuantidadeItensCarrinhoAsync(Guid? idUsuario)
        {
            if (!idUsuario.HasValue) return 0;

            var carrinho = await _repositoryCarrinho.ObterCarrinhoAbertoUsuarioAsync(idUsuario.Value);
            if (carrinho == null) return 0;

            return await _repositoryCarrinhoItem.ContarItensCarrinhoAsync(carrinho.IdCarrinho);
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