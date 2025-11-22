using BROS_ECommerce.Infra.Context;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Controllers
{
    [Route("tenant/logo")]
    public class LogoController : Controller
    {
        private readonly BrosContext _context;

        public LogoController(BrosContext context)
        {
            _context = context;
        }

        [HttpGet("{id:guid}")]
        public IActionResult ObterLogo(Guid id)
        {
            var img = _context.Imagens.FirstOrDefault(x => x.IdImagem == id);
            if (img == null)
                return NotFound();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.CaminhoArquivo);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, img.TipoMime);
        }
    }

}
