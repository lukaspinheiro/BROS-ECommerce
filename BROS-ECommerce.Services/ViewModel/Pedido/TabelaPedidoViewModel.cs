namespace BROS_ECommerce.Services.ViewModel.Pedido
{
    public class TabelaPedidoViewModel
    {
        public Guid IdPedido { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public DateTime DataPedido { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int QuantidadeItens { get; set; }
        public decimal ValorTotal { get; set; }
        public string ObservacoesPedido { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }

        public string DataPedidoFormatada => DataPedido.ToString("dd/MM/yyyy HH:mm");
        public string ValorTotalFormatado => ValorTotal.ToString("F2");
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
}