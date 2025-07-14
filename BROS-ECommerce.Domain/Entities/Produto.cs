using BROS_ECommerce.Domain.Interfaces;

namespace BROS_ECommerce.Domain.Entities
{
    public class Produto : IAggregateRoot
    {
        public Produto() { }

        public Guid IdProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string TituloDescricao { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }

        // Relacionamentos
        public virtual ICollection<ProdutoImagem> ProdutoImagens { get; set; } = new List<ProdutoImagem>();
    }
}