using BROS_ECommerce.Domain.Interfaces;

namespace BROS_ECommerce.Domain.Entities
{
    public class Promocao : IAggregateRoot, ITemTenant
    {
        public Promocao() { }

        public Guid IdPromocao { get; set; }
        public string TenantId { get; set; }
        public Guid IdProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal PercentualDesconto { get; set; }
        public decimal? ValorDesconto { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public virtual Produto Produto { get; set; } = null!;
    }
}