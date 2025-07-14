using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Usuario;

namespace BROS_ECommerce.Web.Areas.Conta.Controllers
{
    [Area("Conta")]
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly IServiceUser _serviceUser;

        public PerfilController(IServiceUser serviceUser)
        {
            _serviceUser = serviceUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var email = User.Identity?.Name;

            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Autenticacao");

            var usuario = await _serviceUser.GetByEmailAsync(email);

            if (usuario == null)
                return RedirectToAction("Login", "Autenticacao");

            var viewModel = new PerfilUsuarioViewModel
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Cpf = usuario.Cpf,
                Nascimento = usuario.Nascimento,
                Genero = usuario.Genero
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("/Conta/Perfil/UsuarioLogado")]
        public IActionResult UsuarioLogado()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null || !identity.IsAuthenticated)
                return Unauthorized();

            var nome = identity.FindFirst("nome")?.Value;
            var email = identity.FindFirst(ClaimTypes.Email)?.Value;

            return Json(new { nome, email });
        }
    }
}
