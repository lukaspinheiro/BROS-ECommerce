using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Produto;

namespace BROS_ECommerce.Services.Services
{
    public class ProdutoService : IServiceProduto
    {
        private static List<ProdutoViewModel> _produtos = new List<ProdutoViewModel>
        {
            new ProdutoViewModel
            {
                Id = Guid.NewGuid(),
                Nome = "Whey Protein",
                Slug = "whey-protein",
                Descricao = "Suplemento de proteína ideal para ganho de massa muscular.",
                ImagemUrl = "/imagens/whey.jpg",
                Preco = 149.99M
            },
            new ProdutoViewModel
            {
                Id = Guid.NewGuid(),
                Nome = "Creatina",
                Slug = "creatina",
                Descricao = "Melhora o desempenho físico em exercícios repetidos de curta duração e alta intensidade.",
                ImagemUrl = "/imagens/creatina.jpg",
                Preco = 149.99M
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
    }

}
