namespace BROS_ECommerce.Services.ViewModel.Estoque
{
    public class TabelaEstoqueViewModel
    {
        public TabelaEstoqueViewModel()
        {
            Estoque = new List<EstoqueViewModel>();
        }
        public TabelaEstoqueViewModel(Guid idEstoque, Guid idProduto, string nome, int quantidade, DateTime ultimaAtualizacao)
        {
            IdEstoque = idEstoque;
            IdProduto = idProduto;
            Nome = nome;
            UltimaAtualizacao = ultimaAtualizacao;
            Quantidade = quantidade;
        }
        public List<EstoqueViewModel> Estoque { get; set; }
        public Guid IdEstoque { get; set; }
        public Guid IdProduto { get; set; }
        public string Nome { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public int Quantidade { get; set; }
    }
}
