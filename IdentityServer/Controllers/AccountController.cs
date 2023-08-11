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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager; 
        private readonly IIdentityServerInteractionService _interaction;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
        }

        [HttpGet("[action]")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? ReturnUrl) 
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
                ModelState.AddModelError("User", "Something went wrong");
                return View(loginViewModel);
            }

            return Redirect(ReturnUrl);
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
}
