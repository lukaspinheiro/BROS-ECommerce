using BROS_ECommerce.Domain.Interfaces;

namespace BROS_ECommerce.Domain.Entities
{
    public class Pedido : IAggregateRoot
    {
        public Pedido() { }

        public Guid IdPedido { get; set; }
        public Guid IdUsuario { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public DateTime DataPedido { get; set; }
        public string Status { get; set; } = "Pendente";
        public decimal ValorSubtotal { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorTotal { get; set; }
        public string? ObservacoesPedido { get; set; }
        public string? ObservacoesInternas { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public string? MotivoCancelamento { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public virtual User Usuario { get; set; } = null!;
        public virtual ICollection<PedidoItem> PedidoItens { get; set; } = new List<PedidoItem>();
    }
}