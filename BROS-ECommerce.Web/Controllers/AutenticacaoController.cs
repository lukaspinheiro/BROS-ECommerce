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
}
