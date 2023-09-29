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
using System;
using RestSharp;

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

                var User = _context.User.SingleOrDefault(u => u.Phone == phone);
                if (User != null)
                {
                    var R = new Random();
                    int Rn = R.Next(1000, 9999);
                    string code = Rn.ToString();
                    var client = new RestClient($"https://portal.amootsms.com/webservice2.asmx/SendWithPattern?UserName=09371552698&Password=dell3porde&Mobile={phone}&PatternCodeID=1871	&PatternValues={code}");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    User.ValidationCode = code;
                    _context.User.Update(User);
                    _context.SaveChanges();
                    ViewBag.Phone = phone;

                    return View("CodeVerification");
                }

                return View("CodeVerification");
            }
            else
            {
                ViewBag.LoginOrRegister = 2;

                User User = new User();
                User.Phone = phone;
                User.ImageName = "Default.png";
                _context.Add(User);
                _context.SaveChanges();

                var R = new Random();
                int Rn = R.Next(1000, 9999);
                string code = Rn.ToString();
                var client = new RestClient($"https://portal.amootsms.com/webservice2.asmx/SendWithPattern?UserName=09371552698&Password=dell3porde&Mobile={phone}&PatternCodeID=1871	&PatternValues={code}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                User.ValidationCode = code;
                _context.User.Update(User);
                _context.SaveChanges();
                ViewBag.Phone = phone;

                return View("CodeVerification");
            }

            return Redirect("/");
        }

        [Route("/FinalSignIn")]
        public IActionResult FinalSignIn(string validationCode, string phone, int loginOrRegister)
        {
            var User = _context.User.SingleOrDefault(u => u.Phone == phone);
            if (User.ValidationCode == validationCode)
            {
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
                return Redirect("/Profile");
            }
            else
            {
                ViewBag.WrongCode = true;
                return View("Index");
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
