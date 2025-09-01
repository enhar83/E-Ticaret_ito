using Data.Abstract;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Areas.Admin.Models;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userModel = new List<UsersWithRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userModel.Add(new UsersWithRoleViewModel
                {
                    Id = user.Id,
                    Name = user.FirstName,
                    Surname =user.LastName,
                    UserName = user.UserName,
                    Phone = user.PhoneNumber,
                    Email = user.Email,
                    Role = roles.Any() ? string.Join(", ", roles) : "Rol Tanımlanmamış"
                });
            }

            return Json(new { data = userModel });
        }
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Json(new
            {
                success = true,
                data = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.UserName,
                    user.PhoneNumber,
                    user.Email,
                    Role = roles.Any() ? string.Join(", ", roles) : "Rol Tanımlanmamış"
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(Guid id, string role)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });

            var currentRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            await _userManager.AddToRoleAsync(user, role);

            return Json(new { success = true });
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UsersWithRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            user.FirstName = model.Name;
            user.LastName = model.Surname;
            user.UserName = model.UserName;
            user.PhoneNumber = model.Phone;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new
            {
                success = false,
                message = string.Join(", ", result.Errors.Select(e => e.Description))
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new
            {
                success = false,
                message = string.Join(", ", result.Errors.Select(e => e.Description))
            });
        }
    }
}
