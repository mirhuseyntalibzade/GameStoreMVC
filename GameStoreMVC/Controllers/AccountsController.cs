using GameStoreMVC.Contexts;
using GameStoreMVC.Models;
using GameStoreMVC.ViewModels.UserVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreMVC.Controllers
{
    public class AccountsController : Controller
    {
        readonly AppDbContext _context;
        readonly SignInManager<AppUser> _signInManager;
        readonly UserManager<AppUser> _userManager;
        public AccountsController(AppDbContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateUserVM userVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userVM);
            }
            AppUser user = new AppUser
            {
                Name = userVM.Name,
                Surname = userVM.Surname,
                UserName = userVM.UserName,
                Email = userVM.Email
            };
            bool emailExists = await _context.Users.AnyAsync(a=>a.Email == userVM.Email);
            if (emailExists)
            {
                ModelState.AddModelError("", "Email has already been registered.");
                return View(userVM);
            }
            if (userVM.Password != userVM.ConfirmPassword)
            {
                ModelState.AddModelError("","Passwords are not matching.");
                return View(userVM);
            }
            var result = await _userManager.CreateAsync(user,userVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code,item.Description);
                }
                return View(userVM);
            }
            return RedirectToAction("Login","Accounts");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserVM userVM)
        {
            AppUser? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userVM.EmailOrUserName || u.UserName == userVM.EmailOrUserName);
            if (!ModelState.IsValid)
            {
                return View(userVM);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "Username or Email is incorrect.");
                return View(userVM);
            }
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, userVM.Password);
            if (!isPasswordValid)
            {
                ModelState.AddModelError("", "Password is incorrect.");
                return View(userVM);
            }
            await _signInManager.SignInAsync(user, isPersistent: true);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
