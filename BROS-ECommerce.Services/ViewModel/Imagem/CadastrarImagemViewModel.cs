using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BROS_ECommerce.Services.ViewModel.Imagem
{
    public class CadastrarImagemViewModel
    {
        [Required(ErrorMessage = "Selecione pelo menos uma imagem")]
        public List<IFormFile> Arquivos { get; set; } = new();

        [MaxLength(255, ErrorMessage = "Texto alternativo não pode exceder 255 caracteres")]
        public string? AltText { get; set; }

        public bool ProcessarMultiplasImagens => Arquivos?.Count > 1;

        
        public static readonly string[] TiposPermitidos = { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
        public static readonly long TamanhoMaximo = 5 * 1024 * 1024; 

        public bool ValidarArquivos(out List<string> erros)
        {
            erros = new List<string>();

            if (Arquivos == null || !Arquivos.Any())
            {
                erros.Add("Selecione pelo menos uma imagem");
                return false;
            }

            foreach (var arquivo in Arquivos)
            {
                if (arquivo.Length == 0)
                {
                    erros.Add($"Arquivo {arquivo.FileName} está vazio");
                    continue;
                }

                if (arquivo.Length > TamanhoMaximo)
                {
                    erros.Add($"Arquivo {arquivo.FileName} excede o tamanho máximo de 5MB");
                }

                if (!TiposPermitidos.Contains(arquivo.ContentType.ToLower()))
                {
                    erros.Add($"Arquivo {arquivo.FileName} não é um tipo de imagem válido");
                }
            }

            return !erros.Any();
        }
    }
}