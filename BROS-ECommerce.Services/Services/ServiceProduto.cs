using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Imagem;
using BROS_ECommerce.Services.ViewModel.Produto;

namespace BROS_ECommerce.Services.Services
{
    public class ProdutoService : IServiceProduto
    {
        private readonly IRepositoryProduto _repositoryProduto;
        private readonly IRepositoryEstoque _repositoryEstoque;
        private readonly IServiceImagem _serviceImagem;
        private readonly IRepositoryCategoriaProduto _repositoryCategoriaProduto;

        public ProdutoService(IRepositoryProduto repositoryProduto, IRepositoryEstoque repositoryEstoque, IServiceImagem serviceImagem, IRepositoryCategoriaProduto repositoryCategoriaProduto)
        {
            _repositoryProduto = repositoryProduto;
            _repositoryEstoque = repositoryEstoque;
            _repositoryCategoriaProduto = repositoryCategoriaProduto;
            _serviceImagem = serviceImagem;
            _repositoryCategoriaProduto = repositoryCategoriaProduto;
        }

        public async Task<List<TabelaProdutoViewModel>> ObterTabelaProdutosAsync()
        {
            var produtos = await _repositoryProduto.ObterTodosComImagensAsync();

            return produtos.Select(p => new TabelaProdutoViewModel
            {
                IdProduto = p.IdProduto,
                Nome = p.Nome,
                Slug = p.Slug,
                TituloDescricao = p.TituloDescricao,
                Descricao = p.Descricao,
                Preco = p.Preco,
                ImagemPrincipal = p.ProdutoImagens
                    .Where(pi => pi.Principal && pi.Imagem.Ativo)
                    .Select(pi => pi.Imagem.CaminhoArquivo)
                    .FirstOrDefault(),
                Imagens = p.ProdutoImagens
                    .Where(pi => pi.Imagem.Ativo)
                    .OrderByDescending(pi => pi.Principal)
                    .ThenBy(pi => pi.Ordem)
                    .Select(pi => pi.Imagem.CaminhoArquivo)
                    .ToList()
            }).ToList();
        }

        public async Task<List<ProdutoViewModel>> ObterTodosAsync()
        {
            var produtos = await _repositoryProduto.ObterTodosComImagensAsync();

            return produtos.Select(p => new ProdutoViewModel
            {
                IdProduto = p.IdProduto,
                Nome = p.Nome,
                Slug = p.Slug,
                TituloDescricao = p.TituloDescricao,
                Descricao = p.Descricao,
                Preco = p.Preco,
                Imagens = p.ProdutoImagens
                    .Where(pi => pi.Imagem.Ativo)
                    .OrderByDescending(pi => pi.Principal)
                    .ThenBy(pi => pi.Ordem)
                    .Select(pi => pi.Imagem.CaminhoArquivo)
                    .ToList()
            }).ToList();
        }

        public List<ProdutoViewModel> ObterTodos()
        {
            var produtos = _repositoryProduto.ObterTodosComImagensAsync().Result;

            return produtos.Select(p => new ProdutoViewModel
            {
                IdProduto = p.IdProduto,
                Nome = p.Nome,
                Slug = p.Slug,
                TituloDescricao = p.TituloDescricao,
                Descricao = p.Descricao,
                Preco = p.Preco,
                Imagens = p.ProdutoImagens
                    .Where(pi => pi.Imagem.Ativo)
                    .OrderByDescending(pi => pi.Principal)
                    .ThenBy(pi => pi.Ordem)
                    .Select(pi => pi.Imagem.CaminhoArquivo)
                    .ToList()
            }).ToList();
        }

        public async Task<ProdutoViewModel?> ObterPorSlugAsync(string slug)
        {
            var produto = await _repositoryProduto.ObterPorSlugComImagensAsync(slug);

            if (produto == null) return null;

            return new ProdutoViewModel
            {
                IdProduto = produto.IdProduto,
                Nome = produto.Nome,
                Slug = produto.Slug,
                TituloDescricao = produto.TituloDescricao,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Imagens = produto.ProdutoImagens
                    .Where(pi => pi.Imagem.Ativo)
                    .OrderByDescending(pi => pi.Principal)
                    .ThenBy(pi => pi.Ordem)
                    .Select(pi => pi.Imagem.CaminhoArquivo)
                    .ToList()
            };
        }

        public ProdutoViewModel? ObterPorSlug(string slug)
        {
            return ObterPorSlugAsync(slug).Result;
        }

        public async Task<ProdutoViewModel?> ObterPorIdAsync(Guid id)
        {
            var produto = await _repositoryProduto.ObterPorIdComImagensAsync(id);

            if (produto == null) return null;

            return new ProdutoViewModel
            {
                IdProduto = produto.IdProduto,
                Nome = produto.Nome,
                Slug = produto.Slug,
                TituloDescricao = produto.TituloDescricao,
                Descricao = produto.Descricao,
                Preco = produto.Preco,

                Imagens = produto.ProdutoImagens
                .Where(pi => pi.Imagem.Ativo)
                .OrderByDescending(pi => pi.Principal)
                .ThenBy(pi => pi.Ordem)
                .Select(pi => pi.Imagem.CaminhoArquivo)
                .ToList(),

                ImagensDetalhadas = produto.ProdutoImagens
                   .Where(pi => pi.Imagem.Ativo)
                   .OrderByDescending(pi => pi.Principal)
                   .ThenBy(pi => pi.Ordem)
                   .Select(pi => new ImagemViewModel
                   {
                       IdImagem = pi.IdImagem,
                       CaminhoArquivo = pi.Imagem.CaminhoArquivo,
                       Principal = pi.Principal
                   })
                   .ToList()
            };

        }

        public async Task Adicionar(CadastrarProdutoViewModel produtoVM)
        {
            var slugExiste = await _repositoryProduto.SlugExisteAsync(produtoVM.Slug);
            if (slugExiste)
            {
                throw new InvalidOperationException($"Já existe um produto com o slug '{produtoVM.Slug}'");
            }

            var produto = new Produto()
            {
                IdProduto = Guid.NewGuid(),
                Nome = produtoVM.Nome,
                Slug = produtoVM.Slug,
                TituloDescricao = produtoVM.TituloDescricao,
                Descricao = produtoVM.Descricao,
                Preco = produtoVM.Preco,
            };

            await _repositoryProduto.AdicionarAsync(produto);
        }

        public async Task AtualizarAsync(ProdutoViewModel produtoVM)
        {
            var produto = await _repositoryProduto.ObterPorIdAsync(produtoVM.IdProduto);

            if (produto == null)
            {
                throw new InvalidOperationException("Produto não encontrado");
            }

            var slugExiste = await _repositoryProduto.SlugExisteAsync(produtoVM.Slug, produtoVM.IdProduto);
            if (slugExiste)
            {
                throw new InvalidOperationException($"Já existe outro produto com o slug '{produtoVM.Slug}'");
            }

            produto.Nome = produtoVM.Nome;
            produto.Slug = produtoVM.Slug;
            produto.TituloDescricao = produtoVM.TituloDescricao;
            produto.Descricao = produtoVM.Descricao;
            produto.Preco = produtoVM.Preco;

            await _repositoryProduto.AtualizarAsync(produto);
        }

        public async Task AtualizarComImagensAsync(IndexProdutoViewModel indexProdutoViewModel)
        {
            var vm = indexProdutoViewModel.cadastrarProdutoViewModel;
            var produto = await _repositoryProduto.ObterPorIdComImagensAsync(vm.IdProduto);

            if (produto == null)
                throw new Exception("Produto não encontrado.");

            var slugExiste = await _repositoryProduto.SlugExisteAsync(vm.Slug, vm.IdProduto);
            if (slugExiste)
                throw new InvalidOperationException($"Já existe outro produto com o slug '{vm.Slug}'");

            produto.Nome = vm.Nome;
            produto.Slug = vm.Slug;
            produto.TituloDescricao = vm.TituloDescricao;
            produto.Descricao = vm.Descricao;
            produto.Preco = vm.Preco;

            await _repositoryProduto.AtualizarAsync(produto);

            List<Guid> novasImagens = new();

            if (vm.Arquivos != null && vm.Arquivos.Any())
            {
                var imagemVM = new CadastrarImagemViewModel
                {
                    Arquivos = vm.Arquivos
                };

                novasImagens = await _serviceImagem.AdicionarMultiplasAsync(imagemVM);
                await _serviceImagem.AssociarImagensAoProdutoAsync(produto.IdProduto, novasImagens);
            }

            if (!string.IsNullOrWhiteSpace(vm.IndiceImagemPrincipal))
            {
                if (vm.IndiceImagemPrincipal.StartsWith("antiga-"))
                {
                    var idImagemAntigaStr = vm.IndiceImagemPrincipal.Replace("antiga-", "");
                    if (Guid.TryParse(idImagemAntigaStr, out Guid idImagemAntiga))
                    {
                        await _serviceImagem.DefinirImagemPrincipalAsync(produto.IdProduto, idImagemAntiga);
                    }
                }
                else if (int.TryParse(vm.IndiceImagemPrincipal, out int indexNova))
                {
                    if (indexNova >= 0 && indexNova < novasImagens.Count)
                    {
                        var idImagemPrincipal = novasImagens[indexNova];
                        await _serviceImagem.DefinirImagemPrincipalAsync(produto.IdProduto, idImagemPrincipal);
                    }
                    else
                    {
                        var imagensOrdenadas = produto.ProdutoImagens
                            .OrderByDescending(pi => pi.Principal)
                            .ThenBy(pi => pi.Ordem)
                            .ToList();

                        if (indexNova >= 0 && indexNova < imagensOrdenadas.Count)
                        {
                            var idImagemPrincipalAntiga = imagensOrdenadas[indexNova].IdImagem;
                            await _serviceImagem.DefinirImagemPrincipalAsync(produto.IdProduto, idImagemPrincipalAntiga);
                        }
                    }
                }
            }
        }

        public async Task ExcluirAsync(Guid id)
        {
            try
            {
                await _repositoryCategoriaProduto.RemoverVinculosPorProdutoAsync(id);

                await _repositoryEstoque.ExcluirPorIdProdutoAsync(id);

                await _repositoryProduto.ExcluirAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao excluir produto: {ex.Message}", ex);
            }
        }


        public async Task<IEnumerable<ProdutoViewModel>> BuscarPorTermoAsync(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return new List<ProdutoViewModel>();

            var produtos = await _repositoryProduto.BuscarPorNomeAsync(termo);

            return produtos.Select(p => new ProdutoViewModel
            {
                IdProduto = p.IdProduto,
                Nome = p.Nome,
                Slug = p.Slug,
                TituloDescricao = p.TituloDescricao,
                Descricao = p.Descricao,
                Preco = p.Preco,
                Imagens = p.ProdutoImagens
                   .Where(pi => pi.Imagem.Ativo)
                    .OrderByDescending(pi => pi.Principal)
                    .ThenBy(pi => pi.Ordem)
                    .Select(pi => $"{pi.Imagem.CaminhoArquivo}")
                    .ToList()


            });
        }

        public async Task<IEnumerable<ProdutoViewModel>> BuscarPorCategoriaAsync(string nomeCategoria)
        {
            if (string.IsNullOrWhiteSpace(nomeCategoria))
                return new List<ProdutoViewModel>();

            var produtos = await _repositoryProduto.BuscarPorCategoriaAsync(nomeCategoria);

            return produtos.Select(p => new ProdutoViewModel
            {
                IdProduto = p.IdProduto,
                Nome = p.Nome,
                Slug = p.Slug,
                TituloDescricao = p.TituloDescricao,
                Descricao = p.Descricao,
                Preco = p.Preco,
                Imagens = p.ProdutoImagens
                    .Where(pi => pi.Imagem.Ativo)
                    .OrderByDescending(pi => pi.Principal)
                    .ThenBy(pi => pi.Ordem)
                    .Select(pi => $"{pi.Imagem.CaminhoArquivo}")
                    .ToList()
            });
        }


        public async Task AdicionarComImagensAsync(IndexProdutoViewModel indexProdutoViewModel)
        {
            var vm = indexProdutoViewModel.cadastrarProdutoViewModel;

            var slugExiste = await _repositoryProduto.SlugExisteAsync(vm.Slug);
            if (slugExiste)
                throw new InvalidOperationException($"Já existe um produto com o slug '{vm.Slug}'");

            var novoProduto = new Produto
            {
                IdProduto = Guid.NewGuid(),
                Nome = vm.Nome,
                Slug = vm.Slug,
                TituloDescricao = vm.TituloDescricao,
                Descricao = vm.Descricao,
                Preco = vm.Preco
            };

            await _repositoryProduto.AdicionarAsync(novoProduto);

            var imagemVM = new CadastrarImagemViewModel
            {
                Arquivos = vm.Arquivos
            };

            var idsImagens = await _serviceImagem.AdicionarMultiplasAsync(imagemVM);

            if (idsImagens == null || !idsImagens.Any())
                throw new Exception("Falha ao salvar imagens");

            await _serviceImagem.AssociarImagensAoProdutoAsync(novoProduto.IdProduto, idsImagens);

            if (!string.IsNullOrWhiteSpace(vm.IndiceImagemPrincipal) &&
                int.TryParse(vm.IndiceImagemPrincipal, out int indice) &&
                indice >= 0 && indice < idsImagens.Count)
            {
                var idPrincipal = idsImagens[indice];
                await _serviceImagem.DefinirImagemPrincipalAsync(novoProduto.IdProduto, idPrincipal);
            }
        }

        public async Task PopularDadosIniciais()
        {
            var produtos = await _repositoryProduto.ObterTodosAsync();

            if (!produtos.Any())
            {
                var produtosIniciais = new List<Produto>
                {
                    new Produto
                    {
                        IdProduto = Guid.NewGuid(),
                        Nome = "Whey Protein",
                        Slug = "whey-protein",
                        TituloDescricao = "Suplemento de Proteína",
                        Descricao = "Whey Protein concentrado para ganho de massa muscular",
                        Preco = 89.90m
                    },
                    new Produto
                    {
                        IdProduto = Guid.NewGuid(),
                        Nome = "Creatina",
                        Slug = "creatina",
                        TituloDescricao = "Suplemento de Creatina",
                        Descricao = "Creatina monohidratada para aumento da força",
                        Preco = 45.90m
                    }
                };

                foreach (var produto in produtosIniciais)
                {
                    await _repositoryProduto.AdicionarAsync(produto);
                }
            }
        }
    }
}