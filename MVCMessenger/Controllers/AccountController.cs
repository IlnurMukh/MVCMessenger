using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.IdentityModel.Tokens;
using MVCMessenger.Interfaces;
using MVCMessenger.Models;
using MVCMessenger.ViewModels;

namespace MVCMessenger.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMessengerRepository _messengerRepository;

        public AccountController(IMessengerRepository messengerRepository)
        {
            _messengerRepository = messengerRepository;
        }
        public async Task<IActionResult> Login()
        {
            if (!Request.Cookies["Id"].IsNullOrEmpty())
            {
                Response.Cookies.Delete("Id");
                Response.Cookies.Delete("Username");
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AccountViewModel accountViewModel)
        {
            
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Unable to access your account. Check the correctness of the entered data";
                return View();
            }

            User? user = _messengerRepository.FindUser(new User()
            {
                Email = accountViewModel.Email,
                Password = accountViewModel.Password
            });
            if (user != null)
            {
                //HttpContext.Session.SetString("Username", user.Username);
                //HttpContext.Session.SetString("Id", user.Id.ToString());
                Response.Cookies.Append("Username", user.Username);
                Response.Cookies.Append("Id", user.Id.ToString());
                return RedirectToAction("Index", "Main");
            }
            ViewBag.Message = "Failed to login. The email or password is incorrect.";
            return View();
        }

        public IActionResult Registration()
        {
            if (!HttpContext.Session.GetString("Username").IsNullOrEmpty())
                return RedirectToAction("Index", "Main");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(AccountViewModel accountViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Failed to create an account. Check the correctness of the entered data";
                return View();
            }

            User user = new User()
            {
                Email = accountViewModel.Email,
                Username = accountViewModel.Username,
                Password = accountViewModel.Password
            };
            if (_messengerRepository.AddUserAsync(user))
            {
                Response.Cookies.Append("Username", user.Username);
                Response.Cookies.Append("Id", user.Id.ToString());
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Failed to register. Try a different username or email address.";
            return View();
        }
        
    }
}
