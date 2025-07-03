namespace BROS_ECommerce.Services.ViewModel.Estoque
{
    public class EstoqueViewModel
    {
        public Guid IdEstoque { get; set; }
        public Guid IdProduto { get; set; }
        public string Nome { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public int Quantidade { get; set; }
    }
}
