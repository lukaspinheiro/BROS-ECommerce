namespace BROS_ECommerce.Services.ViewModel.Carrinho
{
    public class CarrinhoViewModel
    {
        public Guid IdCarrinho { get; set; }
        public Guid? IdUsuario { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Status { get; set; } = "Aberto";
        public List<ItemCarrinhoViewModel> Itens { get; set; } = new List<ItemCarrinhoViewModel>();

        public int QuantidadeTotal => Itens.Sum(i => i.Quantidade);
        public decimal ValorTotal => Itens.Sum(i => i.Subtotal);
        public bool TemItens => Itens.Any();
    }
}