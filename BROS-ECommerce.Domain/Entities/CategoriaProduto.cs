using BROS_ECommerce.Domain.Interfaces;

namespace BROS_ECommerce.Domain.Entities
{
    public class CategoriaProduto : IAggregateRoot, ITemTenant
    {
        public CategoriaProduto() { }

        public Guid IdCategoriaProduto { get; set; }
        public string TenantId { get; set; }
        public Guid IdCategoria { get; set; }
        public Guid IdProduto { get; set; }
        public DateTime DataAssociacao { get; set; }
        public virtual Categoria Categoria { get; set; } = null!;
        public virtual Produto Produto { get; set; } = null!;
    }
}