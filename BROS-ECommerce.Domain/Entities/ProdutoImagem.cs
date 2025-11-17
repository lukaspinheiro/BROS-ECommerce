using BROS_ECommerce.Domain.Interfaces;

namespace BROS_ECommerce.Domain.Entities
{
    public class ProdutoImagem : IAggregateRoot, ITemTenant
    {
        public ProdutoImagem() { }

        public Guid IdProdutoImagem { get; set; }
        public string TenantId { get; set; }
        public Guid IdProduto { get; set; }
        public Guid IdImagem { get; set; }
        public int Ordem { get; set; } = 0;
        public bool Principal { get; set; } = false;
        public DateTime DataAssociacao { get; set; }

        // Relacionamentos
        public virtual Produto Produto { get; set; } = null!;
        public virtual Imagem Imagem { get; set; } = null!;
    }
}