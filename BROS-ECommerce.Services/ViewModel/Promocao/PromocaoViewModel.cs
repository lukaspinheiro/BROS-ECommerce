namespace BROS_ECommerce.Services.ViewModel.Promocao
{
    public class PromocaoViewModel
    {
        public Guid IdPromocao { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public Guid IdProduto { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public string ImagemProduto { get; set; } = string.Empty;
        public decimal PrecoOriginal { get; set; }
        public decimal PrecoPromocional { get; set; }
        public double PercentualDesconto { get; set; }
        public decimal ValorDesconto { get; set; }

    }
}
