using BROS_ECommerce.Domain.Interfaces;

namespace BROS_ECommerce.Domain.Entities
{
    public class PedidoItem : IAggregateRoot, ITemTenant
    {
        public PedidoItem() { }

        public Guid IdPedidoItem { get; set; }
        public string TenantId { get; set; }
        public Guid IdPedido { get; set; }
        public Guid IdProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorTotal { get; set; }

        public virtual Pedido Pedido { get; set; } = null!;
        public virtual Produto Produto { get; set; } = null!;
    }
}