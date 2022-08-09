using CodeRoom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CodeRoom.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Chat()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<AppUser> users = _userManager.Users.ToList();
                return View(users);
            }

            return RedirectToAction("Login", "Account");
        }
    }
}
