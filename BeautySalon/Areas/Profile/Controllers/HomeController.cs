using BeautySalon.Migrations;
using BeautySalon.Models;
using BeautySalon.Models.Context;
using BeautySalon.Models.Tools;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Linq;

namespace BeautySalon.Areas.Profile.Controllers
{
    public class HomeController : Controller
    {
        private readonly SalonContext _context;
        private readonly IWebHostEnvironment _en;
        public HomeController(SalonContext context, IWebHostEnvironment en)
        {
            _context = context;
            _en = en;
        }

        [Authorize]
        [Area("Profile")]
        public IActionResult Index()
        {
            ViewBag.Model = _context.User.Find(int.Parse(User.Identity.GetId()));
            ViewBag.Id = int.Parse(User.Identity.GetId());
            ViewBag.Reservations = _context.Reservations.Where(r => r.UserId == int.Parse(User.Identity.GetId())).OrderByDescending(r => r.DayDate);

            return View();
        }

        [Area("Profile")]
        [Route("/Profile/EditProfileInfo/{id}")]
        public IActionResult EditProfileInfo(int id)
        {
            EditProfileInfoViewModel CurrentInfo = _context.User.Where(u => u.id == id).Select(u => new EditProfileInfoViewModel
            {
                FullName = u.FullName,
                Phone = u.Phone,
                CurrentImage = u.ImageName
            }).Single();

            return View(CurrentInfo);
        }

        [Area("Profile")]
        [Route("/Profile/EditProfileInfo/{id}")]
        [HttpPost]
        public IActionResult EditProfileInfo(int id, EditProfileInfoViewModel profile, string currentImage)
        {
            var User = _context.User.Find(id);
            User.FullName = profile.FullName;
            User.Phone = profile.Phone;

            string ImagePath = "\\Profile\\img\\ProfileImages\\";

            string ThumbPath = "\\Profile\\img\\ProfileThumbnails\\";

            ImageHelper i = new ImageHelper(_en);

            User.ImageName = i.EditImage(profile.ImageName, currentImage, ImagePath, ThumbPath);

            _context.Update(User);

            var HC = _context.HappyClients.SingleOrDefault(h => h.User == id);

            HC.ImageName = User.ImageName;

            _context.Update(HC);
            _context.SaveChanges();

            return Redirect("/Profile");
        }
    }
}
