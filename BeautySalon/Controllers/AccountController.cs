using BeautySalon.Migrations;
using BeautySalon.Models.Context;
using BeautySalon.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Areas;

namespace BeautySalon.Controllers
{
    public class AccountController : Controller
    {
        private readonly SalonContext _context;

        private readonly IWebHostEnvironment _en;
        public AccountController(SalonContext context, IWebHostEnvironment en)
        {
            _context = context;
            _en = en;
        }

        [Route("/Account")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/SignIn")]
        public IActionResult SignIn(string phone)
        {
            if (phone == null)
            {
                ViewBag.NullPhoneNumber = true;
                return View("Index");
            }

            if (_context.Admin.Any(a => a.Phone == phone))
            {
                var SubAdmin = _context.Admin.SingleOrDefault(a => a.Phone == phone);

                if (SubAdmin.AdminRole != 1)
                {
                    ViewBag.SubAdmin = true;
                    ViewBag.Phone = phone;
                    return View("Index");
                }
            }
            else if (_context.User.Any(u => u.Phone == phone))
            {
                ViewBag.LoginOrRegister = 1;
                ViewBag.Phone = phone;
                return View("CodeVerification");
                
            }
            else
            {
                ViewBag.LoginOrRegister = 2;
                ViewBag.Phone = phone;
                return View("CodeVerification");
            }

            return Redirect("/");
        }

        [Route("/FinalSignIn")]
        public IActionResult FinalSignIn(string verificationCode, string phone, int loginOrRegister)
        {
            if(loginOrRegister == 1)
            {
                var User = _context.User.SingleOrDefault(u => u.Phone == phone);
                User.ValidationCode = int.Parse(verificationCode);
                _context.Update(User);
                _context.SaveChanges();

                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,User.id.ToString()),
                        new Claim(ClaimTypes.MobilePhone,User.Phone)
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                HttpContext.SignInAsync(principal, properties);
                return Redirect("/");
            }
            else
            {
                User User = new User();
                User.Phone = phone;
                User.ValidationCode = int.Parse(verificationCode);
                User.ImageName = "Default.png";
                _context.Add(User);
                _context.SaveChanges();

                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,User.id.ToString()),
                        new Claim(ClaimTypes.MobilePhone,User.Phone)
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                HttpContext.SignInAsync(principal, properties);
                return Redirect("/");
            }
        }

            [Route("/LoginSubAdmin")]
        public IActionResult LoginSubAdmin(string password, string phone)
        {
            if (password == null)
            {
                ViewBag.NullPassword = true;
                ViewBag.SubAdmin = true;
                ViewBag.Phone = phone;
                return View("Index");
            }

            var User = _context.Admin.SingleOrDefault(a => a.Phone == phone);

            if (User.Password == password)
            {
                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,User.Id.ToString()),
                        new Claim(ClaimTypes.Name,User.Name),
                        new Claim(ClaimTypes.Actor,User.AdminRole.ToString())
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                HttpContext.SignInAsync(principal, properties);
            }
            else
            {
                ViewBag.InCorrectPassword = true;
                ViewBag.SubAdmin = true;
                ViewBag.Phone = phone;
                return View("Index");
            }
            return RedirectToAction("Index", "Home", new { area = "SubAdmin" });
        }

        [Route("/MainAdminAccount")]
        public IActionResult MainAdminAccount()
        {
            return View();
        }

        [Route("/MainAdminAccount")]
        [HttpPost]
        public IActionResult MainAdminAccount(string phone)
        {
            if (phone == null)
            {
                ViewBag.NullPhoneNumber = true;
                return View();
            }

            if (_context.Admin.Any(a => a.Phone == phone))
            {
                var Admin = _context.Admin.SingleOrDefault(a => a.Phone == phone);

                if (Admin.AdminRole == 1)
                {
                    ViewBag.Admin = true;
                    ViewBag.Phone = phone;
                    return View();
                }
            }
            else
            {
                ViewBag.IncorrectPhoneNumber = true;
                return View();
            }
            return View();
        }

        [Route("/SignInMainAdmin")]
        public IActionResult SignInMainAdmin(string password, string phone)
        {
            if (password == null)
            {
                ViewBag.NullPassword = true;
                ViewBag.Admin = true;
                ViewBag.Phone = phone;
                return View("MainAdminAccount");
            }

            var User = _context.Admin.SingleOrDefault(a => a.Phone == phone);

            if (User.Password == password)
            {
                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,User.Id.ToString()),
                        new Claim(ClaimTypes.Name,User.UserName),
                        new Claim(ClaimTypes.Actor,User.AdminRole.ToString())
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                HttpContext.SignInAsync(principal, properties);
            }
            else
            {
                ViewBag.InCorrectPassword = true;
                ViewBag.Admin = true;
                ViewBag.Phone = phone;
                return View("MainAdminAccount");
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Route("/SignOut")]
        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
