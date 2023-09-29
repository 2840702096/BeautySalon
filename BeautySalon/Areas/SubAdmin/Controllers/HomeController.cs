using BeautySalon.Models.Context;
using BeautySalon.Models.Entities;
using BeautySalon.Models.Tools;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BeautySalon.Models;
using Microsoft.VisualBasic;
using BeautySalon.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Areas.SubAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly SalonContext _context;

        private readonly IWebHostEnvironment _en;

        private readonly IHelpingServices _helpingService;

        public HomeController(SalonContext context, IWebHostEnvironment en, IHelpingServices helpingService)
        {
            _context = context;
            _en = en;
            _helpingService = helpingService;
        }

        [Area("SubAdmin")]
        public IActionResult Index()
        {
            return View();
        }

        #region WorkingTimes

        [Area("SubAdmin")]
        [Route("/SubAdmin/WorkingTimes")]
        public IActionResult WorkingTimes()
        {
            List<WorkingTime> WorkingTimes = _context.WorkingTime.Where(w=>w.AdminId == int.Parse(User.Identity.GetId())).ToList();

            DateTime N = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            ViewBag.Now = N;

            TimeSpan Ts = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            ViewBag.Time = Ts;

            _helpingService.NoneActiveFormerTimes(N, Ts);

            return View(WorkingTimes);
        }

        #region CreateWorkingTime

        [Area("SubAdmin")]
        [Route("/SubAdmin/CreateWorkingTime")]
        public IActionResult CreateWorkingTime()
        {
            ViewBag.Days = _context.WorkingDays.Where(d => d.IsActive == true).ToList();

            return View();
        }

        [Area("SubAdmin")]
        [Route("/SubAdmin/CreateWorkingTime")]
        [HttpPost]
        public IActionResult CreateWorkingTime(CreateWorkingTimeViewModel time)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Days = _context.WorkingDays.Where(d => d.IsActive == true).ToList();

                return View(time);
            }

            int SubAdminId = int.Parse(User.Identity.GetId());

            string[] Day = time.Day.Split(",");

            int DayId = Convert.ToInt32(Day.AsQueryable().First());
            string DayName = Day.AsQueryable().Last();

            WorkingDays WDay = _context.WorkingDays.Find(DayId);

            if (time.StartTime < WDay.StartTime || time.StartTime > WDay.EndTime)
            {
                ViewBag.StartTimeError = true;
                ViewBag.Days = _context.WorkingDays.Where(d => d.IsActive == true).ToList();

                return View(time);
            }

            if (time.EndTime > WDay.EndTime || time.EndTime < WDay.StartTime)
            {
                ViewBag.EndTimeError = true;
                ViewBag.Days = _context.WorkingDays.Where(d => d.IsActive == true).ToList();

                return View(time);
            }

            if (_context.WorkingTime.Any(w => w.IsActive == true && w.AdminId == SubAdminId && w.DayId == DayId && w.StartTime <= time.StartTime && w.EndTime > time.StartTime))
            {
                ViewBag.TheExistTimeAboutStartTime = true;
                ViewBag.Days = _context.WorkingDays.Where(d => d.IsActive == true).ToList();

                return View(time);
            }

            if (_context.WorkingTime.Any(w => w.IsActive == true && w.AdminId == SubAdminId && w.DayId == DayId && w.StartTime < time.EndTime && w.EndTime >= time.EndTime))
            {
                ViewBag.TheExistTimeAboutEndTime = true;
                ViewBag.Days = _context.WorkingDays.Where(d => d.IsActive == true).ToList();

                return View(time);
            }

            if (_context.WorkingTime.Any(w => w.IsActive == true && w.AdminId == SubAdminId && w.DayId == DayId && w.StartTime >= time.StartTime && w.EndTime <= time.EndTime))
            {
                ViewBag.TheExistTimeInThisPeriodOfTime = true;
                ViewBag.Days = _context.WorkingDays.Where(d => d.IsActive == true).ToList();

                return View(time);
            }

            Models.Entities.WorkingTime NewTime = new WorkingTime();

            NewTime.DayId = DayId;
            NewTime.DayName = DayName;
            NewTime.DayDate = _context.WorkingDays.SingleOrDefault(w => w.Id == NewTime.DayId).Date;
            NewTime.StartTime = time.StartTime;
            NewTime.EndTime = time.EndTime;
            NewTime.TimePeriodText = $"{string.Format("{0:00}:{1:00}", time.StartTime.Hours, time.StartTime.Minutes)} - {string.Format("{0:00}:{1:00}", time.EndTime.Hours, time.EndTime.Minutes)}";
            string.Format("{0:00}:{1:00}", time.EndTime.Hours, time.EndTime.Minutes);
            NewTime.IsActive = true;
            NewTime.AdminId = SubAdminId;
            NewTime.AdminName = User.Identity.Name;
            NewTime.JobId = _context.Admin.Find(NewTime.AdminId).SubJobId;
            NewTime.JobName = _context.Admin.Find(NewTime.AdminId).SubJobName;
            NewTime.Price = time.Price;
            NewTime.ReservationCost = time.ReservationCost;
            NewTime.IsReserved = false;

            _context.Add(NewTime);
            _context.SaveChanges();

            return Redirect("/SubAdmin/WorkingTimes");
        }

        #endregion

        #region EditWorkingTime

        [Area("SubAdmin")]
        [Route("/SubAdmin/EditWorkintTime/{id}")]
        public IActionResult EditWorkintTime(int id)
        {
            EditWorkingTimeViewModel CurrentTime = _context.WorkingTime.Where(w => w.Id == id).Select(w => new EditWorkingTimeViewModel
            {
                DayId = w.DayId,
                DayName = w.DayName,
                StartTime = w.StartTime,
                EndTime = w.EndTime,
                Price = w.Price,
                ReservationCost = w.ReservationCost
            }).Single();

            ViewBag.Days = _context.WorkingDays.Where(w => w.Id != CurrentTime.DayId).ToList();

            return View(CurrentTime);
        }

        [Area("SubAdmin")]
        [Route("/SubAdmin/EditWorkintTime/{id}")]
        [HttpPost]
        public IActionResult EditWorkintTime(int id, EditWorkingTimeViewModel updatedTime)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedTime);
            }

            int SubAdminId = int.Parse(User.Identity.GetId());

            string[] Day = updatedTime.Day.Split(",");

            int DayId = Convert.ToInt32(Day.AsQueryable().First());
            string DayName = Day.AsQueryable().Last();

            WorkingDays WDay = _context.WorkingDays.Find(DayId);

            if (updatedTime.StartTime < WDay.StartTime || updatedTime.StartTime > WDay.EndTime)
            {
                ViewBag.StartTimeError = true;
                ViewBag.Days = _context.WorkingDays.Where(d => d.IsActive == true).ToList();

                return View(updatedTime);
            }

            if (updatedTime.EndTime > WDay.EndTime || updatedTime.EndTime < WDay.StartTime)
            {
                ViewBag.EndTimeError = true;
                ViewBag.Days = _context.WorkingDays.Where(d => d.IsActive == true).ToList();

                return View(updatedTime);
            }

            if (_context.WorkingTime.Any(w => w.IsActive == true && w.Id != id && w.AdminId == SubAdminId && w.DayId == DayId && w.StartTime <= updatedTime.StartTime && w.EndTime >= updatedTime.StartTime))
            {
                ViewBag.TheExistTimeAboutStartTime = true;
                ViewBag.Days = _context.WorkingDays.Where(d => d.IsActive == true).ToList();

                return View(updatedTime);
            }

            if (_context.WorkingTime.Any(w => w.IsActive == true && w.Id != id && w.AdminId == SubAdminId && w.DayId == DayId && w.StartTime < updatedTime.EndTime && w.EndTime >= updatedTime.EndTime))
            {
                ViewBag.TheExistTimeAboutEndTime = true;
                ViewBag.Days = _context.WorkingDays.Where(d => d.IsActive == true).ToList();

                return View(updatedTime);
            }

            if (_context.WorkingTime.Any(w => w.IsActive == true && w.Id != id && w.AdminId == SubAdminId && w.DayId == DayId && w.StartTime >= updatedTime.StartTime && w.EndTime <= updatedTime.EndTime))
            {
                ViewBag.TheExistTimeInThisPeriodOfTime = true;
                ViewBag.Days = _context.WorkingDays.Where(d => d.IsActive == true).ToList();

                return View(updatedTime);
            }

            Models.Entities.WorkingTime CurrentTime = _context.WorkingTime.Find(id);

            CurrentTime.DayId = Convert.ToInt32(Day.AsQueryable().First());
            CurrentTime.DayName = Day.AsQueryable().Last();
            CurrentTime.DayDate = _context.WorkingDays.SingleOrDefault(w => w.Id == CurrentTime.DayId).Date;
            CurrentTime.StartTime = updatedTime.StartTime;
            CurrentTime.EndTime = updatedTime.EndTime;
            CurrentTime.TimePeriodText = $"{string.Format("{0:00}:{1:00}", updatedTime.StartTime.Hours, updatedTime.StartTime.Minutes)} - {string.Format("{0:00}:{1:00}", updatedTime.EndTime.Hours, updatedTime.EndTime.Minutes)}";
            CurrentTime.Price = updatedTime.Price;
            CurrentTime.ReservationCost = updatedTime.ReservationCost;

            _context.Update(CurrentTime);
            _context.SaveChanges();

            return Redirect("/SubAdmin/WorkingTimes");
        }

        #endregion

        #region Activation

        [Area("SubAdmin")]
        [Route("/SubAdmin/ActivateWorkingTime/{id}")]
        public IActionResult ActivateWorkingTime(int id)
        {
            Models.Entities.WorkingTime Time = _context.WorkingTime.Find(id);
            Time.IsActive = true;

            _context.Update(Time);
            _context.SaveChanges();

            return Redirect($"/SubAdmin/WorkingTimes");
        }

        [Area("SubAdmin")]
        [Route("/SubAdmin/MakeWorkingTimeNoneActive/{id}")]
        public IActionResult MakeWorkingTimeNoneActive(int  id)
        {
            Models.Entities.WorkingTime Time = _context.WorkingTime.Find(id);
            Time.IsActive = false;

            _context.Update(Time);
            _context.SaveChanges();

            return Redirect($"/SubAdmin/WorkingTimes");
        }

        #endregion

        #region DeleteWorkingTime


        [Area("SubAdmin")]
        [Route("/SubAdmin/DeleteWorkingTime/{id}")]
        public IActionResult DeleteWorkingTime(int id)
        {
            //Todo Make If Before Deleting

            _context.Entry(_context.WorkingTime.Find(id)).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();

            return Redirect("/SubAdmin/WorkingTimes");
        }

        #endregion

        #endregion

        #region Reservations

        [Area("SubAdmin")]
        [Route("/SubAdmin/Reservations")]
        public IActionResult Reservations()
        {
            ViewBag.Model = _context.Reservations.AsNoTracking().ToList();
            return View();
        }

        [Area("SubAdmin")]
        [Route("/SubAdmin/ReservationDescription/{id}")]
        public IActionResult ReservationDescription(int id)
        {
            ViewBag.Description = _context.Reservations.Find(id).Description;

            return View();
        }

        #endregion
    }
}
