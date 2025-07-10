using BROS_ECommerce.Domain.Interfaces;

namespace BROS_ECommerce.Domain.Entities
{
    public class Imagem : IAggregateRoot
    {
        public Imagem() { }

        public Guid IdImagem { get; set; }
        public string NomeArquivo { get; set; } = string.Empty;
        public string CaminhoArquivo { get; set; } = string.Empty;
        public long TamanhoArquivo { get; set; }
        public string TipoMime { get; set; } = string.Empty;
        public string? AltText { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; } = true;

        public virtual ICollection<ProdutoImagem> ProdutoImagens { get; set; } = new List<ProdutoImagem>();
    }
}