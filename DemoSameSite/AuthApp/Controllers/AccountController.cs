using AuthApp.Models;
using AuthApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthApp.Controllers
{
    public class AccountController : Controller
    {
        private UserContext db;
        public AccountController(UserContext context)
        {
            db = context;
            DatabaseInitialize();
        }

        private void DatabaseInitialize()
        {
            if (!db.Roles.Any())
            {
                string adminRoleName = "admin";
                string userRoleName = "user";

                string adminName = "admin@mail.ru";
                string adminPass = "123465";

                Role adminRole = new Role { Name = adminRoleName };
                Role userRole = new Role { Name = userRoleName };

                db.Roles.Add(adminRole);
                db.Roles.Add(userRole);

                db.Users.Add(new User { Email = adminName, Password = adminPass, Role = adminRole });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("Form3Ds", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
                if (user == null)
                {
                    user = new User { Email = model.Email, Password = model.Password };
                    Role role = await db.Roles.FirstOrDefaultAsync(x => x.Name == "user");
                    if (role != null)
                        user.Role = role;
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");

            }
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}