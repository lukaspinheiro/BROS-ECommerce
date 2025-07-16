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

            if (!idUsuario.HasValue)
            {
                throw new UnauthorizedAccessException("Usuário não autenticado");
            }

            var produto = await _repositoryProduto.ObterPorIdAsync(idProduto);
            if (produto == null)
            {
                Console.WriteLine($"[SERVICE] ERRO: Produto não encontrado!");
                throw new ArgumentException("Produto não encontrado");
            }

            Console.WriteLine($"[SERVICE] Produto encontrado: {produto.Nome}");

            var estoque = await _repositoryEstoque.ObterPorIdProdutoAsync(idProduto);

            if (estoque == null)
            {
                Console.WriteLine($"[SERVICE] ERRO: Produto não possui registro de estoque!");
                Console.WriteLine($"[SERVICE] Produto: {produto.Nome} (ID: {idProduto})");
                throw new InvalidOperationException($"O produto '{produto.Nome}' ainda não foi adicionado ao estoque. Contate o administrador.");
            }

            Console.WriteLine($"[SERVICE] Estoque encontrado: {estoque.Quantidade} unidades");

            var carrinho = await _repositoryCarrinho.ObterCarrinhoAbertoUsuarioAsync(idUsuario.Value);

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

            Console.WriteLine($"[SERVICE] Verificando item existente no carrinho...");
            var itemExistente = await _repositoryCarrinhoItem.ObterPorCarrinhoEProdutoAsync(carrinho.IdCarrinho, idProduto);
            Console.WriteLine($"[SERVICE] Item existente: {itemExistente != null}");

            int quantidadeFinal;

            if (itemExistente != null)
            {
                quantidadeFinal = itemExistente.Quantidade + quantidade;
                Console.WriteLine($"[SERVICE] Quantidade atual no carrinho: {itemExistente.Quantidade}");
                Console.WriteLine($"[SERVICE] Quantidade final calculada: {quantidadeFinal}");
            }
            else
            {
                quantidadeFinal = quantidade;
                Console.WriteLine($"[SERVICE] Novo item - Quantidade final: {quantidadeFinal}");
            }

            if (quantidadeFinal > estoque.Quantidade)
            {
                Console.WriteLine($"[SERVICE] ERRO: Estoque insuficiente!");
                Console.WriteLine($"[SERVICE] Solicitado: {quantidadeFinal}, Disponível: {estoque.Quantidade}");
                throw new InvalidOperationException($"Estoque insuficiente. Disponível: {estoque.Quantidade}");
            }

            if (itemExistente != null)
            {
                Console.WriteLine($"[SERVICE] Atualizando quantidade do item existente...");
                itemExistente.Quantidade = quantidadeFinal;
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

                await _repositoryCarrinhoItem.AdicionarItemAsync(novoItem);
                Console.WriteLine($"[SERVICE] Item adicionado com sucesso!");
            }

            Console.WriteLine($"[SERVICE] Obtendo carrinho atualizado...");
            var carrinhoAtualizado = await ObterCarrinhoPorIdAsync(carrinho.IdCarrinho) ?? throw new InvalidOperationException("Erro ao obter carrinho atualizado");

            Console.WriteLine($"[SERVICE] Carrinho atualizado com sucesso!");
            Console.WriteLine($"[SERVICE] =================================");

            return carrinhoAtualizado;
        }

        public async Task<(bool EstoqueValido, string Mensagem, CarrinhoViewModel CarrinhoAtualizado)> ValidarEAjustarEstoqueCarrinhoAsync(Guid idUsuario)
        {
            var carrinho = await ObterCarrinhoUsuarioAsync(idUsuario);
            if (carrinho == null || !carrinho.TemItens)
            {
                return (true, "Carrinho vazio", new CarrinhoViewModel());
            }

            bool houveMudancas = false;
            var problemas = new List<string>();

            foreach (var item in carrinho.Itens)
            {
                var estoque = await _repositoryEstoque.ObterPorIdProdutoAsync(item.IdProduto);

                if (estoque == null)
                {
                    var carrinhoEntity = await _repositoryCarrinho.ObterCarrinhoAbertoUsuarioAsync(idUsuario);
                    if (carrinhoEntity != null)
                    {
                        var itemEntity = await _repositoryCarrinhoItem.ObterPorCarrinhoEProdutoAsync(carrinhoEntity.IdCarrinho, item.IdProduto);
                        if (itemEntity != null)
                        {
                            await _repositoryCarrinhoItem.RemoverItemAsync(itemEntity.IdCarrinhoItem);
                        }
                    }
                    problemas.Add($"{item.Nome} foi removido (sem estoque)");
                    houveMudancas = true;
                    continue;
                }

                if (item.Quantidade > estoque.Quantidade)
                {
                    var carrinhoEntity = await _repositoryCarrinho.ObterCarrinhoAbertoUsuarioAsync(idUsuario);
                    if (carrinhoEntity != null)
                    {
                        var itemEntity = await _repositoryCarrinhoItem.ObterPorCarrinhoEProdutoAsync(carrinhoEntity.IdCarrinho, item.IdProduto);
                        if (itemEntity != null)
                        {
                            if (estoque.Quantidade == 0)
                            {
                                await _repositoryCarrinhoItem.RemoverItemAsync(itemEntity.IdCarrinhoItem);
                                problemas.Add($"{item.Nome} foi removido (esgotado)");
                            }
                            else
                            {
                                itemEntity.Quantidade = estoque.Quantidade;
                                await _repositoryCarrinhoItem.AtualizarItemAsync(itemEntity);
                                problemas.Add($"{item.Nome} teve quantidade ajustada para {estoque.Quantidade}");
                            }
                        }
                    }
                    houveMudancas = true;
                }
            }

            var carrinhoAtualizado = await ObterCarrinhoUsuarioAsync(idUsuario) ?? new CarrinhoViewModel();

            if (houveMudancas)
            {
                var mensagem = "Alguns itens foram ajustados: " + string.Join(", ", problemas);
                return (false, mensagem, carrinhoAtualizado);
            }

            return (true, "Estoque validado com sucesso", carrinhoAtualizado);
        }

        public async Task<CarrinhoViewModel> AtualizarQuantidadeAsync(Guid idCarrinho, Guid idProduto, int quantidade)
        {
            if (quantidade <= 0)
            {
                await RemoverProdutoAsync(idCarrinho, idProduto);
                return await ObterCarrinhoPorIdAsync(idCarrinho) ?? new CarrinhoViewModel();
            }

            var item = await _repositoryCarrinhoItem.ObterPorCarrinhoEProdutoAsync(idCarrinho, idProduto);
            if (item == null)
            {
                throw new InvalidOperationException("Produto não encontrado no carrinho");
            }

            var estoque = await _repositoryEstoque.ObterPorIdProdutoAsync(idProduto);
            if (estoque != null && estoque.Quantidade < quantidade)
            {
                throw new InvalidOperationException($"Estoque insuficiente. Disponível: {estoque.Quantidade}");
            }

            item.Quantidade = quantidade;
            await _repositoryCarrinhoItem.AtualizarItemAsync(item);

            return await ObterCarrinhoPorIdAsync(idCarrinho) ?? new CarrinhoViewModel();
        }

        public async Task<bool> RemoverProdutoAsync(Guid idCarrinho, Guid idProduto)
        {
            var item = await _repositoryCarrinhoItem.ObterPorCarrinhoEProdutoAsync(idCarrinho, idProduto);
            if (item == null) return false;

            await _repositoryCarrinhoItem.RemoverItemAsync(item.IdCarrinhoItem);
            return true;
        }

        public async Task<bool> LimparCarrinhoAsync(Guid idCarrinho)
        {
            var itens = await _repositoryCarrinhoItem.ObterItensPorCarrinhoAsync(idCarrinho);
            foreach (var item in itens)
            {
                await _repositoryCarrinhoItem.RemoverItemAsync(item.IdCarrinhoItem);
            }
            return true;
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

            return await AdicionarProdutoAsync(idUsuario, idProduto, quantidade);
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