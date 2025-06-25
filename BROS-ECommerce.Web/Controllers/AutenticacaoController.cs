
using Microsoft.AspNetCore.Mvc;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Web.ViewModels;

namespace BROS_ECommerce.Web.Controllers
{
    public class AutenticacaoController : Controller
    {
        private readonly IServiceUser _userService;
        private readonly ILogger<AutenticacaoController> _logger;

        public AutenticacaoController(IServiceUser userService, ILogger<AutenticacaoController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuario, string senha)
        {
            
            return RedirectToAction("Index", "Home");
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