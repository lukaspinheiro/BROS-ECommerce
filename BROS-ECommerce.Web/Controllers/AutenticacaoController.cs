using Microsoft.AspNetCore.Mvc;

public class AutenticacaoController : Controller
{
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
        return View();
    }

    [HttpPost]
    public IActionResult Cadastro(string email, string cpf, string nome, string nascimento, string senha, string genero)
    {
        
        return RedirectToAction("Login");
    }

}
