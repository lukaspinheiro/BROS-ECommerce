namespace BROS_ECommerce.Services.ViewModel.Imagem
{
    public class IndexImagemViewModel
    {
        public IndexImagemViewModel()
        {
            Filtro = new FiltroImagemViewModel();
            Imagens = new List<ImagemViewModel>();
            CadastrarImagem = new CadastrarImagemViewModel();
        }

        public FiltroImagemViewModel Filtro { get; set; }
        public List<ImagemViewModel> Imagens { get; set; }
        public CadastrarImagemViewModel CadastrarImagem { get; set; }
        public int TotalItens { get; set; }
        public int PaginaAtual { get; set; } = 1;
        public int ItensPorPagina { get; set; } = 12;
        public int TotalPaginas => (int)Math.Ceiling((double)TotalItens / ItensPorPagina);
        public long TamanhoTotal { get; set; }
        public string TamanhoTotalFormatado => FormatarTamanho(TamanhoTotal);

        private static string FormatarTamanho(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            int counter = 0;
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }
            return $"{number:n1} {suffixes[counter]}";
        }
    }

    public class FiltroImagemViewModel
    {
        public string? Nome { get; set; }
        public string? TipoMime { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool? Ativo { get; set; } = true;
        public bool? ApenasNaoAssociadas { get; set; }
    }
}