using BeautySalon.Models;
using BeautySalon.Models.Context;
using BeautySalon.Models.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SalonContext _context;

        private readonly IWebHostEnvironment _en;

        private readonly IHelpingServices _helpingService;

        public HomeController(ILogger<HomeController> logger, SalonContext context, IWebHostEnvironment en, IHelpingServices helpingService)
        {
            _logger = logger;
            _context = context;
            _en = en;
            _helpingService = helpingService;
        }

        public IActionResult Index()
        {
            ViewBag.Sliders = _context.Sliders.Where(s => s.IsActive == true).ToList();
            ViewBag.Specifications = _context.Specifications.Where(s => s.IsActive == true).ToList();
            ViewBag.Services = _context.Job.Where(j => j.IsActive == true && j.Name != "صاحب آرایشگاه").ToList();
            ViewBag.Gallery = _context.Gallery.Where(g => g.IsActive == true).OrderByDescending(g => g.CreateDate).Take(6).ToList();
            ViewBag.Employees = _context.Admin.Where(a => a.IsActive == true && a.AdminRole == 2).ToList();
            ViewBag.WorkingDays = _context.WorkingDays.Where(w => w.IsActive == true).ToList();
            ViewBag.Weblogs = _context.Weblogs.Where(w => w.IsActive == true).ToList();
            ViewBag.Partners = _context.Partner.Where(p => p.IsActive == true).ToList();

            DateTime N = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            _helpingService.NoneActiveFormerDays(N);

            var Services = _helpingService.GetServices();
            List<SelectListItem> NewList = new List<SelectListItem>();

            foreach (var Item in Services)
            {
                if (_context.Admin.Any(a=>a.SubJobId == int.Parse(Item.Value) && a.IsActive == true))
                {
                    NewList.Add(Item);
                }
            }

            if (NewList.Count != 0)
            {
                ViewBag.SelectBoxServices = new SelectList(NewList, "Value", "Text");

                var Personel = _helpingService.GetPersonel(int.Parse(NewList.First().Value)).ToList();

                List<SelectListItem> NewPersonelList = new List<SelectListItem>();

                foreach (var Item in Personel)
                {
                    if (_context.WorkingTime.Any(w=>w.IsActive == true && w.AdminId == int.Parse(Item.Value)))
                    {
                        NewPersonelList.Add(Item);
                    }
                }

                if (NewPersonelList.Count != 0)
                {
                    ViewBag.Personel = new SelectList(NewPersonelList, "Value", "Text");
                    var Day = _helpingService.GetDay(int.Parse(NewPersonelList.First().Value));

                    ViewBag.Day = new SelectList(Day, "Value", "Text");
                    int DayId = int.Parse(Day.First().Value);

                    var WTime = _helpingService.GetWorkingTimeDueToPersonel(int.Parse(NewPersonelList.First().Value), DayId);
                    ViewBag.WorkingTime = new SelectList(WTime, "Value", "Text");
                }
                else
                {

                }
            }
            else
            {

            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Team

        [Route("/TeamDetail/{id}")]
        public IActionResult TeamDetail(int id)
        {
            var Detail = _context.Admin.Find(id);
            return View(Detail);
        }

        #endregion
    }
}
