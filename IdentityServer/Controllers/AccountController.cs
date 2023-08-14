using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;  
        private IIdentityServerInteractionService _interaction;

        public AccountController(
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, 
            IIdentityServerInteractionService interaction)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interaction = interaction;
        }

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
}
