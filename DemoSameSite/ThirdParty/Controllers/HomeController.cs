using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ThirdParty.Models;

namespace ThirdParty.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            var url = Request.Headers["Referer"];
            url = string.Concat(url, "Home/Check3Ds");
            var model = new RedirectModel
            {
                Url = url
            };
            return View(model);
        }
    }
}
