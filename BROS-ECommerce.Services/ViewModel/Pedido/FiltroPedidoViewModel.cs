namespace BROS_ECommerce.Services.ViewModel.Pedido
{
    public class FiltroPedidoViewModel
    {
        public string? NumeroPedido { get; set; }
        public string? Status { get; set; }
        public string? NomeCliente { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}