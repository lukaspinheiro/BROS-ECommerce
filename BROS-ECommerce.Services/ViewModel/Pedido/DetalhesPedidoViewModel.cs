using BROS_ECommerce.Services.ViewModel.Pedido;

namespace BROS_ECommerce.Services.ViewModel.Pedido
{
    public class DetalhesPedidoViewModel
    {
        public Guid IdPedido { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public DateTime DataPedido { get; set; }
        public string Status { get; set; } = string.Empty;
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

        public ClientePedidoViewModel Cliente { get; set; } = new();
        public List<ItemPedidoViewModel> Itens { get; set; } = new();

        public string DataPedidoFormatada => DataPedido.ToString("dd/MM/yyyy HH:mm");
        public string ValorTotalFormatado => ValorTotal.ToString("C");
        public string StatusFormatado => Status switch
        {
            "Pendente" => "Pendente",
            "Confirmado" => "Confirmado",
            "Processando" => "Processando",
            "Enviado" => "Enviado",
            "Entregue" => "Entregue",
            "Cancelado" => "Cancelado",
            _ => Status
        };
        public string StatusCor => Status switch
        {
            "Pendente" => "warning",
            "Confirmado" => "info",
            "Processando" => "primary",
            "Enviado" => "success",
            "Entregue" => "success",
            "Cancelado" => "danger",
            _ => "secondary"
        };
    }

    public class ClientePedidoViewModel
    {
        public Guid IdUsuario { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
    }

    public class ItemPedidoViewModel
    {
        public Guid IdPedidoItem { get; set; }
        public Guid IdProduto { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorTotal { get; set; }
    }
}