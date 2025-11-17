
using BROS_ECommerce.Domain.Interfaces;

namespace BROS_ECommerce.Domain.Entities
{
    public class CarrinhoItem : IAggregateRoot, ITemTenant
    {
        public CarrinhoItem() { }

        public Guid IdCarrinhoItem { get; set; }
        public string TenantId { get; set; }
        public Guid IdCarrinho { get; set; }
        public Guid IdProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        
        public virtual Carrinho Carrinho { get; set; } = null!;
        public virtual Produto Produto { get; set; } = null!;
    }
}