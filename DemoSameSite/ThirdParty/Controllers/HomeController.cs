using Microsoft.AspNetCore.Mvc;
using ThirdParty.Models;

namespace ThirdParty.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public IActionResult Index([FromForm] string postbackurl)
        {
            var model = new RedirectModel
            {
                Url = postbackurl
            };
            return View(model);
        }
    }
}
