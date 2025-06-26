using Microsoft.AspNetCore.Mvc;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Web.ViewModels;
using BROS_ECommerce.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace BROS_ECommerce.Web.Controllers
{
    public class AutenticacaoController : Controller
    {
        private readonly IServiceUser _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AutenticacaoController> _logger;

        public AutenticacaoController(
            IServiceUser userService,
            IAuthenticationService authenticationService,
            ILogger<AutenticacaoController> logger)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            
            if (_authenticationService.IsAuthenticated(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

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

                    
                    var loginSuccess = await _authenticationService.SignInAsync(HttpContext, user, model.LembrarSenha);

                    if (loginSuccess)
                    {
                        _logger.LogInformation("Sign-in realizado com sucesso");
                        TempData["SuccessMessage"] = $"Bem-vindo, {user.Nome}!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        _logger.LogError("Falha no sign-in");
                        TempData["ErrorMessage"] = "Erro interno durante o login. Tente novamente.";
                        
                        model.Senha = string.Empty;
                        return View(model);
                    }
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

        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> Logout()
        {
            try
            {
                _logger.LogInformation("=== LOGOUT REALIZADO ===");
                _logger.LogInformation($"Usuário: {HttpContext.User?.Identity?.Name}");

                await _authenticationService.SignOutAsync(HttpContext);

                TempData["SuccessMessage"] = "Logout realizado com sucesso!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante logout");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Cadastro()
        {
            
            if (_authenticationService.IsAuthenticated(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new UserRegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(UserRegisterViewModel model)
        {
            _logger.LogInformation("=== INICIANDO CADASTRO ===");
            _logger.LogInformation($"Email: {model.Email}");
            _logger.LogInformation($"CPF: {model.Cpf}");
            _logger.LogInformation($"Nome: {model.Nome}");

            try
            {
                
                if (string.IsNullOrWhiteSpace(model.Email) ||
                    string.IsNullOrWhiteSpace(model.Cpf) ||
                    string.IsNullOrWhiteSpace(model.Nome) ||
                    model.Nascimento == default(DateTime) ||
                    string.IsNullOrWhiteSpace(model.Senha) ||
                    string.IsNullOrWhiteSpace(model.Genero))
                {
                    _logger.LogWarning("Campos obrigatórios não preenchidos");
                    TempData["ErrorMessage"] = "Todos os campos são obrigatórios.";
                    return View(model);
                }

                
                var cpfLimpo = new string(model.Cpf.Where(char.IsDigit).ToArray());
                _logger.LogInformation($"CPF original: {model.Cpf}");
                _logger.LogInformation($"CPF limpo: {cpfLimpo}");
                _logger.LogInformation($"CPF length: {cpfLimpo.Length}");

                
                if (!await _userService.IsEmailAvailableAsync(model.Email))
                {
                    _logger.LogWarning($"Email já cadastrado: {model.Email}");
                    TempData["ErrorMessage"] = "Este email já está cadastrado.";
                    return View(model);
                }

                if (!await _userService.IsCpfAvailableAsync(model.Cpf))
                {
                    _logger.LogWarning($"CPF já cadastrado: {model.Cpf}");
                    TempData["ErrorMessage"] = "Este CPF já está cadastrado.";
                    return View(model);
                }

                _logger.LogInformation("Disponibilidade OK, criando usuário...");

               
                var user = await _userService.CreateUserAsync(
                    email: model.Email,
                    cpf: model.Cpf,
                    nome: model.Nome,
                    nascimento: model.Nascimento,
                    senha: model.Senha,
                    genero: model.Genero
                );

                _logger.LogInformation($"Usuário criado com sucesso! ID: {user.IdUser}");
                TempData["SuccessMessage"] = "Cadastro realizado com sucesso! Faça login para continuar.";
                return RedirectToAction("Login");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Erro de validação no cadastro");
                TempData["ErrorMessage"] = ex.Message;
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro de operação no cadastro");
                TempData["ErrorMessage"] = ex.Message;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral no cadastro");
                TempData["ErrorMessage"] = "Erro ao realizar cadastro. Tente novamente.";
                return View(model);
            }
        }
    }
}