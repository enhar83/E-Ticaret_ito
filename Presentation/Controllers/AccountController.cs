using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.AccountViewModels;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid) //modeldeki data annotationa uyduğunu kontrol eder
            {
                var user = new AppUser
                {
                    Id = Guid.NewGuid(),
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    UserName = registerModel.UserName,
                    Email = registerModel.Email,
                    PhoneNumber = registerModel.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, registerModel.Password); //kullanıcı dbye kaydolur, şifre ise hashlenir
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("User"))
                        await _roleManager.CreateAsync(new AppRole { Name = "User" });

                    await _userManager.AddToRoleAsync(user, "User");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Json(new { success = true, message = "Aramıza hoşgeldin! Giriş sayfasına yönlendiriliyorsunuz..." });
                }

                var errors = result.Errors.Select(e => e.Description).ToList();
                return Json(new { success = false, errors });
            }

            return Json(new { success = false, errors = new[] { "Geçersiz form verisi" } });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = new[] { "Eksik veya hatalı giriş." } });

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Giriş başarılı, ana sayfaya yönlendiriliyorsunuz..." });
                }
            }

            return Json(new { success = false, errors = new[] { "Kullanıcı adı veya şifre hatalı." } });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
