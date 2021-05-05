using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Praktikaaa.Models;
using Praktikaaa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Praktikaaa.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsersContext db;
        public AccountController(UsersContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.Name != null)
            {
                return RedirectToAction("AdminPanel", "Account");
            }
            ViewBag.User = User.Identity.Name;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id != null)
                {
                    Messenger messeng = await db.Messengers.FirstOrDefaultAsync(p => p.Id == id);
                    if (messeng != null)
                    {
                        db.Messengers.Remove(messeng);
                        await db.SaveChangesAsync();
                        return RedirectToAction("AdminPanel");
                    }
                }

            }
            catch
            {
                ViewBag.Error = "Ошибка при удалении";
                return RedirectToAction("AdminPanel");
            }
            ViewBag.Error = "Ошибка при удалении";
            return RedirectToAction("AdminPanel");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email);
                    ViewBag.User = User.Identity.Name;
                    return RedirectToAction("AdminPanel", "Account");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            ViewBag.User = User.Identity.Name;
            return View(model);
        }
        [HttpGet]

        [Authorize]
        public IActionResult Register()
        {
            ViewBag.User = User.Identity.Name;
            return View();
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult AdminPanel()
        {
            ViewBag.User = User.Identity.Name;
            return View(db.Messengers);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Editing(int? id)
        {
            try
            {
                if (id != null)
                {
                    var messeng = await db.Messengers.FirstOrDefaultAsync(p => p.Id == id);
                    ViewBag.User = User.Identity.Name;
                    ViewBag.Messeng = messeng;
                    ViewBag.MessengId = id;
                    return View();
                }
                ViewBag.Error = "Неизвемтная ошибка";
                return RedirectToAction("AdminPanel", "Account");
            }
            catch
            {
                ViewBag.Error = "Неизвемтная ошибка";
                return RedirectToAction("AdminPanel", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Editing(Messenger messeng)
        {
            try
            {
                db.Messengers.Update(messeng);
                await db.SaveChangesAsync();
                ViewBag.User = User.Identity.Name;
                return RedirectToAction("AdminPanel", "Account");
            }
            catch
            {
                ViewBag.Error = "Ошибка";
                return RedirectToAction("AdminPanel", "Account");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Moder()
        {
            if (User.Identity.Name == "Admin")
            {
                ViewBag.User = User.Identity.Name;
                return View(db.Users);
            }
            else
            {
                return RedirectToAction("AdminPanel", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddModer(User user)
        {
            try
            {
                if (user != null && User.Identity.Name == "Admin")
                {
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Moder");
                }
            }
            catch
            {
                ViewBag.Error = "Ошибка при добавлении модератора";
                return RedirectToAction("Moder");
            }
            ViewBag.Error = "Ошибка при добавлении модератора";
            return RedirectToAction("Moder");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            try
            {
                if (id != null && User.Identity.Name == "Admin")
                {
                    User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                    if (user != null)
                    {
                        db.Users.Remove(user);
                        await db.SaveChangesAsync();
                        return RedirectToAction("Moder");
                    }
                }

            }
            catch
            {
                ViewBag.Error = "Ошибка при удалении модератора";
                return RedirectToAction("Moder");
            }
            ViewBag.Error = "Ошибка при удалении модератора";
            return RedirectToAction("Moder");
        }

        [HttpGet]
        public async Task<IActionResult> Delit(int? id)
        {
            try
            {
                if (id != null && User.Identity.Name == "Admin")
                {
                    Messenger messeng = await db.Messengers.FirstOrDefaultAsync(p => p.Id == id);
                    if (messeng != null)
                    {
                        db.Messengers.Remove(messeng);
                        await db.SaveChangesAsync();
                        return RedirectToAction("AdminPanel");
                    }
                }

            }
            catch
            {
                ViewBag.Error = "Ошибка при удалении модератора";
                return RedirectToAction("AdminPanel");
            }
            ViewBag.Error = "Ошибка при удалении модератора";
            return RedirectToAction("AdminPanel");
        }
    }
}
