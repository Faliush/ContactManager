using IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;

[Route("[controller]")]
public class AccountController : Controller
{
    [HttpGet("[action]")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("[action]")]
    public IActionResult Login(LoginViewModel loginViewModel ,string? ReturnUrl) 
    {
        return View(loginViewModel);
    }

    [HttpGet("[action]")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("[action]")]
    public IActionResult Register(RegisterViewModel registerViewModel)
    {
        return View(registerViewModel);
    }
}
