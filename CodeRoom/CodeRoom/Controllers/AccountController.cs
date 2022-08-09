using CodeRoom.DAL;
using CodeRoom.Models;
using CodeRoom.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static CodeRoom.Helpers.Helper;

namespace CodeRoom.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = new AppUser
            {
                Firstname = registerVM.FirstName,
                Surname = registerVM.Surname,
                Email = registerVM.Email,
                UserName = registerVM.Username,

            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Member.ToString());




            return RedirectToAction("login");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string ReturnUrl)
        {

            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or password Invalid.");
                return View(loginVM);
            }



            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account is Blocked");
                return View(loginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password Invalid.");
                return View(loginVM);
            }

            var roles = await _userManager.GetRolesAsync(appUser);

            if (ReturnUrl != null)
            {
                foreach (var item in roles)
                {
                    if (item == "Admin")
                    {
                        return RedirectToAction("Index", "dashboard", new { area = "Admin" });
                    }
                    else
                    {
                        if (ReturnUrl == "register")
                        {
                            return RedirectToAction("chat", "home");
                        }
                        return Redirect(ReturnUrl); ;
                    }
                }
            }


            return RedirectToAction("chat", "home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }



        public async Task CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            };
        }
    }
}
