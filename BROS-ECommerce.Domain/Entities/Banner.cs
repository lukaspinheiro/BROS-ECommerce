using BROS_ECommerce.Domain.Interfaces;

namespace BROS_ECommerce.Domain.Entities
{
    public class Banner : IAggregateRoot, ITemTenant
    {
        public Guid IdBanner { get; set; }
        public string TenantId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Link { get; set; }

        public Guid? IdImagem { get; set; }
        public virtual Imagem? Imagem { get; set; }
        public string? CaminhoImagem { get; set; }   // ADICIONAR
        public int Ordem { get; set; }

        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
    }

}
