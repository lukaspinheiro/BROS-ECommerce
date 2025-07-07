using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Produto;
using BROS_ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BROS_ECommerce.Domain.Interfaces.Repository;

namespace BROS_ECommerce.Services.Services
{
    public class ProdutoService : IServiceProduto
    {
        private readonly IRepositoryProduto _repositoryProduto;
        private readonly IRepositoryEstoque _repositoryEstoque;

        public ProdutoService(IRepositoryProduto repositoryProduto, IRepositoryEstoque repositoryEstoque)
        {
            _repositoryProduto = repositoryProduto;
            _repositoryEstoque = repositoryEstoque;
        }

        public async Task<List<TabelaProdutoViewModel>> ObterTabelaProdutosAsync()
        {
            var produtos = await _repositoryProduto.ObterTodosAsync();

            return produtos.Select(p => new TabelaProdutoViewModel
            {
                IdProduto = p.IdProduto,
                Nome = p.Nome,
                Slug = p.Slug,
                TituloDescricao = p.TituloDescricao,
                Descricao = p.Descricao,
                Preco = p.Preco
            }).ToList();
        }

        public async Task<List<ProdutoViewModel>> ObterTodosAsync()
        {
            var produtos = await _repositoryProduto.ObterTodosAsync();

            return produtos.Select(p => new ProdutoViewModel
            {
                IdProduto = p.IdProduto,
                Nome = p.Nome,
                Slug = p.Slug,
                TituloDescricao = p.TituloDescricao,
                Descricao = p.Descricao,
                Preco = p.Preco,
                Imagens = new List<string>()
            }).ToList();
        }

        public List<ProdutoViewModel> ObterTodos()
        {
            var produtos = _repositoryProduto.ObterTodosAsync().Result;

            return produtos.Select(p => new ProdutoViewModel
            {
                IdProduto = p.IdProduto,
                Nome = p.Nome,
                Slug = p.Slug,
                TituloDescricao = p.TituloDescricao,
                Descricao = p.Descricao,
                Preco = p.Preco,
                Imagens = new List<string>()
            }).ToList();
        }

        public async Task<ProdutoViewModel?> ObterPorSlugAsync(string slug)
        {
            var produto = await _repositoryProduto.ObterPorSlugAsync(slug);

            if (produto == null) return null;

            return new ProdutoViewModel
            {
                IdProduto = produto.IdProduto,
                Nome = produto.Nome,
                Slug = produto.Slug,
                TituloDescricao = produto.TituloDescricao,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Imagens = new List<string>()
            };
        }

        public ProdutoViewModel? ObterPorSlug(string slug)
        {
            return ObterPorSlugAsync(slug).Result;
        }

        public async Task<ProdutoViewModel?> ObterPorIdAsync(Guid id)
        {
            var produto = await _repositoryProduto.ObterPorIdAsync(id);

            if (produto == null) return null;

            return new ProdutoViewModel
            {
                IdProduto = produto.IdProduto,
                Nome = produto.Nome,
                Slug = produto.Slug,
                TituloDescricao = produto.TituloDescricao,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Imagens = new List<string>()
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

            //try
            //{
            //    var estoque = new Estoque(produto.IdProduto, 1); 
            //    await _repositoryEstoque.AdicionarAsync(estoque);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Erro ao criar estoque para produto {produto.Nome}: {ex.Message}");
            //}
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

        public async Task ExcluirAsync(Guid id)
        {
            try
            {
                await _repositoryEstoque.ExcluirPorIdProdutoAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir estoque do produto {id}: {ex.Message}");
            }

            await _repositoryProduto.ExcluirAsync(id);
        }

        public async Task PopularDadosIniciais()
        {
            var totalProdutos = await _repositoryProduto.ContarTotalAsync();

            if (totalProdutos == 0)
            {
                var produtosIniciais = new List<Produto>
                {
                    new Produto
                    {
                        IdProduto = Guid.NewGuid(),
                        Nome = "(TOP) Whey Protein Concentrado (1KG) - Growth Supplements",
                        Slug = "whey-protein-growth-1kg",
                        TituloDescricao = "WHEY PROTEIN GROWTH. PROTEÍNA DO SORO DO LEITE PURA.",
                        Descricao = "Whey protein Growth fornece proteínas para quem deseja hipertrofia e definição muscular. Rico em aminoácidos essenciais.",
                        Preco = 149.99M
                    },
                    new Produto
                    {
                        IdProduto = Guid.NewGuid(),
                        Nome = "Creatina Monohidratada 250g - Growth Supplements",
                        Slug = "creatina-monohidratada-250g",
                        TituloDescricao = "CREATINA MONOHIDRATADA PURO MICRONIZADA.",
                        Descricao = "Melhora o desempenho físico em exercícios repetidos de curta duração e alta intensidade.",
                        Preco = 89.99M
                    },
                    new Produto
                    {
                        IdProduto = Guid.NewGuid(),
                        Nome = "BCAA 2400 - 60 Cápsulas",
                        Slug = "bcaa-2400-60-capsulas",
                        TituloDescricao = "BCAA DE ALTA QUALIDADE PARA RECUPERAÇÃO MUSCULAR.",
                        Descricao = "Aminoácidos de cadeia ramificada para recuperação e crescimento muscular.",
                        Preco = 79.99M
                    }
                };

                foreach (var produto in produtosIniciais)
                {
                    await _repositoryProduto.AdicionarAsync(produto);

                    var estoque = new Estoque(produto.IdProduto, 1);
                    await _repositoryEstoque.AdicionarAsync(estoque);
                }
            }
        }
    }
}