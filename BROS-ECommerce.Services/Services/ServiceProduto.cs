using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Produto;
using BROS_ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.Services
{
    public class ProdutoService : IServiceProduto
    {
        private static List<ProdutoViewModel> _produtos = new List<ProdutoViewModel>
        {
            new ProdutoViewModel
            {
                IdProduto = Guid.NewGuid(),
                Nome = "(TOP) Whey Protein Concentrado (1KG) - Growth Supplements",
                Slug = "whey-protein",
                TituloDescricao = "WHEY PROTEIN GROWTH. PROTEÍNA DO SORO DO LEITE PURA.",
                Descricao = "Whey protein Growth fornece proteínas para quem deseja hipertrofia e definição muscular.\r\n\r\nIdeal porque é um suplemento de alto valor biológico com grande concentração de proteínas e aminoácidos essenciais é também rico em Glutamina, BCAA (incluindo Leucina).",
                Preco = 149.99M,
                Imagens = new List<string> { "https://www.gsuplementos.com.br/upload/produto/layout/185/image01-interna.webp", "https://cdn.awsli.com.br/2500x2500/2690/2690062/produto/254626589/whey-isolado-growth-natural-tabela-ay38azpe7k.png" }
            },
            new ProdutoViewModel
            {
                IdProduto = Guid.NewGuid(),
                Nome = "Creatina Monohidratada 250g - Growth Supplements",
                Slug = "creatina",
                TituloDescricao = "CREATINA MONOHIDRATADA PURO MICRONIZADA.",
                Descricao = "Melhora o desempenho físico em exercícios repetidos de curta duração e alta intensidade.",
                Preco = 149.99M,
                Imagens = new List<string> { "https://www.gsuplementos.com.br/upload/produto/layout/72/produto1-mono-250-v3.webp", "https://a-static.mlcdn.com.br/800x560/creatina-pura-250g-creapure-growth-supplements/suplementosaz/7987155342/447f6778c2727252091c79df2860f344.jpeg" }
            }

        };

        public List<ProdutoViewModel> ObterTodos()
        {
            return _produtos;
        }

        public ProdutoViewModel? ObterPorSlug(string slug)
        {
            return _produtos.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
        }

        public async Task Adicionar(CadastrarProdutoViewModel ProdutoVM)
        {
            //var ultimaOrdem = await _repositoryProduto.ObterUltimaOrdemProdutoAsync();
            //var novaOrdem = (ultimaOrdem ?? 0) + 1;

            var produto = new Produto()
            {
                Nome = ProdutoVM.Nome,
                Slug = ProdutoVM.Slug,
                TituloDescricao = ProdutoVM.TituloDescricao,
                Descricao = ProdutoVM.Descricao,
                Preco = ProdutoVM.Preco,
            };
            //await _repositoryProduto.AdicionarAsync(produto);
            return ;
        }

    }

}
