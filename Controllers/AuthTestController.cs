using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class AuthTestController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult signUp()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View("Login");

        }
    }
}
