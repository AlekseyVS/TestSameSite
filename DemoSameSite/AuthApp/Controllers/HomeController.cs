using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthApp.Controllers
{

    public class HomeController : Controller
    {
        // [AllowAnonymous]
        [Authorize(Roles = "admin,user")]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
                string result = $"name = {User.Identity.Name}, role = {role}";
                return View("Form3Ds");
            }
            return Content("No auth");
        }

        [Authorize(Roles = "admin,user")]
        public IActionResult Form3Ds()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Check3Ds([FromBody]string redirectValue)
        {
            return View();
        }
    }
}