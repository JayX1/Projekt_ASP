using Microsoft.AspNetCore.Mvc;
using ASP_projekt.Services;
using ASP_projekt.Models;
using ASP_projekt.Interfaces;

namespace ASP_projekt.Controllers
{
    public class LoginController : Controller
    {

        private readonly ICustomAuthService _authService;

        public LoginController(ICustomAuthService authService)
        {
            _authService = authService;
        }


        [HttpGet]
        public IActionResult OnLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OnLogin(User user)
        {
            if (_authService.ValidateUser(user.Username, user.Password))
            {
                HttpContext.Response.Cookies.Append("IsAuthenticated", "true");
                return RedirectToAction("Index", "Superhero");
            }

            else
            {
                user.Error = "Wrong user name or password";
                return View();
            }
        }
    }
}
