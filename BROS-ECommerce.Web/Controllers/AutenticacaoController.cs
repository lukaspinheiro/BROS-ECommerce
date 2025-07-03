using Microsoft.AspNetCore.Mvc;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Web.ViewModels;
using BROS_ECommerce.Web.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace BROS_ECommerce.Web.Controllers
{
    public class AutenticacaoController : Controller
    {
        private readonly IServiceUser _userService;
        private readonly JwtService _jwtService;
        private readonly ILogger<AutenticacaoController> _logger;

        public AutenticacaoController(
            IServiceUser userService,
            JwtService jwtService,
            ILogger<AutenticacaoController> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new UserLoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            _logger.LogInformation("=== TENTATIVA DE LOGIN ===");
            _logger.LogInformation($"Email: {model.Usuario}");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido no login");
                model.Senha = string.Empty;
                return View(model);
            }

            try
            {
                var user = await _userService.AuthenticateUserAsync(model.Usuario, model.Senha);

                if (user != null)
                {
                    _logger.LogInformation($"Usuário autenticado com sucesso: {user.Email} (ID: {user.IdUser})");

                    var token = _jwtService.GenerateToken(user);

                    return Json(new
                    {
                        success = true,
                        token,
                        nome = user.Nome,
                        email = user.Email
                    });
                }
                else
                {
                    _logger.LogWarning($"Credenciais inválidas para: {model.Usuario}");
                    TempData["ErrorMessage"] = "Email ou senha inválidos.";
                    model.Senha = string.Empty;
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante tentativa de login");
                TempData["ErrorMessage"] = "Erro interno. Tente novamente mais tarde.";
                model.Senha = string.Empty;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View(new UserRegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(UserRegisterViewModel model)
        {
            _logger.LogInformation("=== INICIANDO CADASTRO ===");
            _logger.LogInformation($"Email: {model.Email}");

            try
            {
                if (string.IsNullOrWhiteSpace(model.Email) ||
                    string.IsNullOrWhiteSpace(model.Cpf) ||
                    string.IsNullOrWhiteSpace(model.Nome) ||
                    model.Nascimento == default ||
                    string.IsNullOrWhiteSpace(model.Senha) ||
                    string.IsNullOrWhiteSpace(model.Genero))
                {
                    TempData["ErrorMessage"] = "Todos os campos são obrigatórios.";
                    return View(model);
                }

                if (!await _userService.IsEmailAvailableAsync(model.Email))
                {
                    TempData["ErrorMessage"] = "Este email já está cadastrado.";
                    return View(model);
                }

                if (!await _userService.IsCpfAvailableAsync(model.Cpf))
                {
                    TempData["ErrorMessage"] = "Este CPF já está cadastrado.";
                    return View(model);
                }

                var user = await _userService.CreateUserAsync(
                    email: model.Email,
                    cpf: model.Cpf,
                    nome: model.Nome,
                    nascimento: model.Nascimento,
                    senha: model.Senha,
                    genero: model.Genero
                );

                TempData["SuccessMessage"] = "Cadastro realizado com sucesso!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar usuário");
                TempData["ErrorMessage"] = "Erro ao realizar cadastro.";
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            // Apenas simbólico. O frontend deve remover o token manualmente.
            _logger.LogInformation("Logout realizado (frontend deve remover o token)");
            return Ok(new { message = "Logout realizado com sucesso." });
        }
    }
}
