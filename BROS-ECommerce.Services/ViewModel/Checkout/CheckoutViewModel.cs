using System.ComponentModel.DataAnnotations;

namespace BROS_ECommerce.Services.ViewModel.Checkout
{
    public class CheckoutViewModel
    {
        public ContatoViewModel Contato { get; set; } = new();
        public EnderecoViewModel Endereco { get; set; } = new();
        public string MetodoPagamento { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal Frete { get; set; }
        public decimal Total { get; set; }
    }

    public class ContatoViewModel
    {
        [Required(ErrorMessage = "Nome completo é obrigatório")]
        [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [StringLength(255, ErrorMessage = "E-mail deve ter no máximo 255 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [StringLength(15, ErrorMessage = "Telefone deve ter no máximo 15 caracteres")]
        [RegularExpression(@"^\(\d{2}\)\s\d{4,5}-\d{4}$", ErrorMessage = "Formato inválido. Use: (11) 99999-9999")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(14, ErrorMessage = "CPF deve ter no máximo 14 caracteres")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Formato inválido. Use: 999.999.999-99")]
        public string Cpf { get; set; } = string.Empty;
    }

    public class EnderecoViewModel
    {
        [Required(ErrorMessage = "CEP é obrigatório")]
        [StringLength(9, ErrorMessage = "CEP deve ter no máximo 9 caracteres")]
        [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "Formato inválido. Use: 99999-999")]
        public string Cep { get; set; } = string.Empty;

        [Required(ErrorMessage = "Logradouro é obrigatório")]
        [StringLength(200, ErrorMessage = "Logradouro deve ter no máximo 200 caracteres")]
        public string Logradouro { get; set; } = string.Empty;

        [Required(ErrorMessage = "Número é obrigatório")]
        [StringLength(10, ErrorMessage = "Número deve ter no máximo 10 caracteres")]
        public string Numero { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Complemento deve ter no máximo 100 caracteres")]
        public string Complemento { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bairro é obrigatório")]
        [StringLength(100, ErrorMessage = "Bairro deve ter no máximo 100 caracteres")]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cidade é obrigatória")]
        [StringLength(100, ErrorMessage = "Cidade deve ter no máximo 100 caracteres")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "Estado é obrigatório")]
        [StringLength(2, ErrorMessage = "Estado deve ter 2 caracteres")]
        public string Estado { get; set; } = string.Empty;
    }
}