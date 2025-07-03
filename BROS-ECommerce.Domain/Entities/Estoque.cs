using BROS_ECommerce.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Domain.Entities
{
    public class Estoque : IAggregateRoot
    {
        public Estoque() { }

        public Estoque(Guid idProduto, int quantidade)
        {
            IdEstoque = Guid.NewGuid();
            IdProduto = idProduto;
            Quantidade = quantidade;
            UltimaAtualizacao = DateTime.UtcNow;
        }

        [Key]
        public Guid IdEstoque { get; set; } = Guid.NewGuid();

        [Required]
        public Guid IdProduto { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime UltimaAtualizacao { get; set; } = DateTime.UtcNow;

        public virtual Produto Produto { get; set; }

        public void AtualizarQuantidade(int novaQuantidade)
        {
            Quantidade = novaQuantidade;
            UltimaAtualizacao = DateTime.UtcNow;
        }

        public void AdicionarQuantidade(int quantidade)
        {
            Quantidade += quantidade;
            UltimaAtualizacao = DateTime.UtcNow;
        }

        public void RemoverQuantidade(int quantidade)
        {
            if (Quantidade >= quantidade)
            {
                Quantidade -= quantidade;
                UltimaAtualizacao = DateTime.UtcNow;
            }
            else
            {
                throw new InvalidOperationException($"Quantidade insuficiente em estoque. Disponível: {Quantidade}, Solicitado: {quantidade}");
            }
        }

        public bool TemEstoqueSuficiente(int quantidadeSolicitada)
        {
            return Quantidade >= quantidadeSolicitada;
        }
    }
}