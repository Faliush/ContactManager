using IdentityServer.Data;
using IdentityServer.Data.Base;
using IdentityServer.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityServer.Controllers;

[Route("[controller]")]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager; 
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IConfiguration _configuration;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IIdentityServerInteractionService interaction,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _configuration = configuration;
    }

    [HttpGet("[action]")]
    public IActionResult Login(string ReturnUrl)
    {
        return View(new LoginViewModel() { ReturnUrl = ReturnUrl });
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel) 
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(e => e.Errors).Select(m => m.ErrorMessage);
            return View(loginViewModel);
        }

        var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

        if(user is null)
        {
            ModelState.AddModelError("User", "User not found");
            return View(loginViewModel);
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("Sign in", "Something went wrong");
            return View(loginViewModel);
        }

        return Redirect(loginViewModel.ReturnUrl);
    }

    [HttpGet("[action]")]
    public IActionResult Register(string ReturnUrl)
    {
        return View(new RegisterViewModel() { ReturnUrl = ReturnUrl });
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
            return View(registerViewModel);

        var alreadyRegistered = await _userManager.FindByEmailAsync(registerViewModel.Email);

        if(alreadyRegistered is not null)
        {
            ModelState.AddModelError("User", "User with this email is already exist");
            return View(registerViewModel);
        }

        if (!registerViewModel.AgreeWithPolitics)
        {
            ModelState.AddModelError("User", "User must be agree with company politics");
            return View(registerViewModel);
        }

        var user = new ApplicationUser()
        {
            UserName = registerViewModel.UserName,
            Email = registerViewModel.Email,
            PhoneNumber = registerViewModel.Phone
        };

        var registerResutlt = await _userManager.CreateAsync(user, registerViewModel.Password);
        if (!registerResutlt.Succeeded)
        {
            ModelState.AddModelError("Registration", "Something went wrong");
            return View(registerViewModel);
        }

        var claimList = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, registerViewModel.UserName),
            new Claim(ClaimTypes.Email, registerViewModel.Email),
            new Claim(ClaimTypes.NameIdentifier, registerViewModel.Email),
            new Claim(ClaimTypes.Role, AppData.UserRoleName)
        };

        if (registerViewModel.Password == _configuration["Secret:AdministratorPassword"])
            claimList.Add(new Claim(ClaimTypes.Role, AppData.AdministratorRoleName));

        var addClaimsResult = await _userManager.AddClaimsAsync(user, claimList);
        if (!addClaimsResult.Succeeded)
        {
            ModelState.AddModelError("User", addClaimsResult.Errors.Select(e => e.Description).First());
            return View(registerViewModel);
        }

        await _signInManager.SignInAsync(user, false);

        return Redirect(registerViewModel.ReturnUrl);
    }

    [Route("[action]")]
    public async Task<IActionResult> Logout(string logoutId)
    {
        await _signInManager.SignOutAsync();
        var logoutResult = await _interaction.GetLogoutContextAsync(logoutId);

        if (string.IsNullOrEmpty(logoutResult.PostLogoutRedirectUri))
            return RedirectToAction("Login", "Account");

        return Redirect(logoutResult.PostLogoutRedirectUri);
    }
}
