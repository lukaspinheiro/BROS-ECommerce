namespace BROS_ECommerce.Services.ViewModel.Imagem
{
    public class ImagemViewModel
    {
        public Guid IdImagem { get; set; }
        public string NomeArquivo { get; set; } = string.Empty;
        public string CaminhoArquivo { get; set; } = string.Empty;
        public long TamanhoArquivo { get; set; }
        public string TipoMime { get; set; } = string.Empty;
        public string? AltText { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; } = true;
        public int QuantidadeProdutos { get; set; } = 0;
        public string TamanhoFormatado => FormatarTamanho(TamanhoArquivo);

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
}