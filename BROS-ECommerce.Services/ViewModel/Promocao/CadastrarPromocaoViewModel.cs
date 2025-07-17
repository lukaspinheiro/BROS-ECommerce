namespace BROS_ECommerce.Services.ViewModel.Promocao
{
    public class CadastrarPromocaoViewModel
    {
        public Guid? IdPromocao { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public Guid IdProduto { get; set; }
        public decimal? PercentualDesconto { get; set; }
        public decimal? ValorDesconto { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}
