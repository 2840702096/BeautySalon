using BeautySalon.Migrations;
using BeautySalon.Models;
using BeautySalon.Models.Context;
using BeautySalon.Models.Entities;
using BeautySalon.Models.Services.Interfaces;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BeautySalon.Controllers
{
    public class ServiceController : Controller
    {
        private SalonContext _context;
        private IHelpingServices _helpingService;

        public ServiceController(SalonContext context, IHelpingServices helpingService)
        {
            _context = context;
            _helpingService = helpingService;
        }

        [Route("/Services/{id}")]
        public IActionResult Index(int id)
        {
            ViewBag.Model = _context.SubJob.AsNoTracking().Where(s => s.ParentId == id && s.IsActive == true).ToList();

            return View();
        }

        [Route("/Services/SingleService/{id}")]
        public IActionResult SingleService(int id)
        {
            ViewBag.Model = _context.SubJob.AsNoTracking().SingleOrDefault(s => s.Id == id);

            return View();
        }

        #region Reservation

        [Route("/Services/ReserveService/{id}")]
        public IActionResult ReserveService(int id)
        {

            DateTime N = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            ViewBag.Now = N;

            TimeSpan Ts = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            ViewBag.Time = Ts;

            _helpingService.NoneActiveFormerTimes(N, Ts);

            var Personel = _helpingService.GetPersonel(id).ToList();
            List<SelectListItem> newlist = new List<SelectListItem>();

            foreach (var Item in Personel)
            {
                if (_context.WorkingTime.Any(w => w.AdminId == int.Parse(Item.Value) && w.IsActive == true))
                {
                    newlist.Add(Item);
                }
            }

            if (newlist.Count != 0)
            {
                ViewBag.Personel = new SelectList(newlist, "Value", "Text");
                var Day = _helpingService.GetDay(int.Parse(newlist.First().Value));

                ViewBag.Day = new SelectList(Day, "Value", "Text");
                int DayId = int.Parse(Day.First().Value);

                var WTime = _helpingService.GetWorkingTimeDueToPersonel(int.Parse(newlist.First().Value), DayId);
                ViewBag.WorkingTime = new SelectList(WTime, "Value", "Text");
            }
            else
            {
                ViewBag.Model = _context.SubJob.AsNoTracking().SingleOrDefault(s => s.Id == id);
                ViewBag.ThereIsNoServices = true;
                return View("SingleService");
            }
            return View();
        }

        [Route("/Services/WorkingDaySettings/{id}")]
        public IActionResult WorkingDaySettings(int id)
        {
            var WorkingDay = _helpingService.GetDay(id);

            HttpContext.Session.SetInt32("porsonid", id);
            return Json(new SelectList(WorkingDay, "Value", "Text"));
        }

        [Route("/Services/WorkingTimeSettings/{id}")]
        public IActionResult WorkingTimeSettings(int id)
        {
            int porsonid = HttpContext.Session.GetInt32("porsonid").Value;
            var WorkingTime = _helpingService.GetWorkingTimeDueToPersonel(porsonid, id);

            return Json(new SelectList(WorkingTime, "Value", "Text"));
        }

        [Route("/Services/PersonelSettings/{id}")]
        public IActionResult PersonelSettings(int id)
        {
            var Personel = _helpingService.GetPersonel(id).ToList();

            return Json(new SelectList(Personel, "Value", "Text"));
        }

        #region CreateAReservation

        [Route("/Services/CreateAReservation")]
        public IActionResult CreateAReservation( ReservationViewModel reservation)
        {
            if(!ModelState.IsValid)
            {
                return Redirect("/Home/Index");
            }

            WorkingTime WT = _context.WorkingTime.Find(reservation.TimeId);

            int UserId = int.Parse(User.Identity.GetId());

            var user = _context.User.Find(UserId);
            if(user.FullName == null)
            {
                ViewBag.ImPerfectProfile = true;
                return View("ReserveService");
            }
            Reservations NewReservation = new Reservations()
            {
                UserId = UserId,
                WorkingTimeId = reservation.TimeId,
                Status = 0,
                Description = reservation.Description,
                Price = int.Parse(WT.Price),
                ReservationCost = int.Parse(WT.ReservationCost),
                JobId = WT.JobId,
                JopName = WT.JobName,
                AdminId = WT.AdminId,
                AdminName = WT.AdminName,
                StartTime = WT.StartTime,
                EndTime = WT.EndTime,
                DayDate = WT.DayDate,
                DayName = WT.DayName,
            };

            _context.Add(NewReservation);

            if(_context.Reservations.Any(r=>r.WorkingTimeId == WT.Id))
            {
                WT.IsReserved = true;

                _context.Update(WT);

                int Score = _context.SubJob.Find(WT.JobId).Score;

                user.Score = Score;

                _context.Update(user);
            }

            _context.SaveChanges();

            return Redirect("/");
        }

        #endregion

        #endregion
    }
}
