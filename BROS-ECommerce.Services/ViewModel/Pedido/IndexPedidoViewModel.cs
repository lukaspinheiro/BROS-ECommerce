namespace BROS_ECommerce.Services.ViewModel.Pedido
{
    public class IndexPedidoViewModel
    {
        public IndexPedidoViewModel()
        {
            Filtro = new FiltroPedidoViewModel();
            Tabela = new List<TabelaPedidoViewModel>();
        }

        public IndexPedidoViewModel(FiltroPedidoViewModel filtro, List<TabelaPedidoViewModel> tabela)
        {
            Filtro = filtro;
            Tabela = tabela;
        }

        public FiltroPedidoViewModel Filtro { get; set; } = new FiltroPedidoViewModel();
        public ICollection<TabelaPedidoViewModel> Tabela { get; set; } = new List<TabelaPedidoViewModel>();
        public int TotalPedidos => Tabela.Count;
        public int PedidosPendentes => Tabela.Count(p => p.Status == "Pendente");
        public int PedidosConfirmados => Tabela.Count(p => p.Status == "Confirmado");
        public int PedidosEntregues => Tabela.Count(p => p.Status == "Entregue");
        public decimal ValorTotalPedidos => Tabela.Sum(p => p.ValorTotal);
    }
}