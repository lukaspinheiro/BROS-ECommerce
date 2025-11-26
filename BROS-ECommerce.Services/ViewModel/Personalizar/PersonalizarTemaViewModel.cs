using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.ViewModel.Personalizar;

public class PersonalizarLojaViewModel
{
    public string? CorMenu { get; set; }
    public string? CorTextoMenu { get; set; }

    public string? CorFundo { get; set; }
    public string? CorTexto { get; set; }

    public string? CorMenuInferior { get; set; }
    public string? CorTextoMenuInferior { get; set; }

    public IFormFile? LogoArquivo { get; set; }
}

