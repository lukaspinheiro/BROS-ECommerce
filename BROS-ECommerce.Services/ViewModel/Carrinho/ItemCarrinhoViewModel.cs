namespace BROS_ECommerce.Services.ViewModel.Carrinho
{
    public class ItemCarrinhoViewModel
    {
        public Guid IdCarrinhoItem { get; set; }
        public Guid IdCarrinho { get; set; }
        public Guid IdProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;

        public decimal Subtotal => Quantidade * PrecoUnitario;
        public string PrecoFormatado => PrecoUnitario.ToString("C2");
        public string SubtotalFormatado => Subtotal.ToString("C2");
    }
}