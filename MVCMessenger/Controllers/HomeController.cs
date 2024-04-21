using Microsoft.AspNetCore.Mvc;
using MVCMessenger.Models;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;

namespace MVCMessenger.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //HttpContext.Session.SetString("Username", String.Empty);
        }

        public IActionResult Index()
        {
            if (!Request.Cookies["Id"].IsNullOrEmpty())
                return RedirectToAction("Index", "Main");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
