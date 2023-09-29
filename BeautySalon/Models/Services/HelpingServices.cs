using BeautySalon.Migrations;
using BeautySalon.Models.Entities;
using BeautySalon.Models.Services.Interfaces;
using System.Collections.Generic;
using BeautySalon.Models.Context;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace BeautySalon.Models.Services
{
    public class HelpingServices : IHelpingServices
    {
        private SalonContext _context;

        public HelpingServices(SalonContext context)
        {
            _context = context;
        }

        public List<SelectListItem> GetDay(int id)
        {
            var f = _context.WorkingTime.Where(w => w.IsActive == true && w.AdminId == id).ToList();
            var c = f.GroupBy(w => w.DayId);
            List<SelectListItem> select = new List<SelectListItem>();

            foreach (var item in c)
            {
                string a = item.Key.ToString();
                string bb = item.First().DayName; 

                SelectListItem n = new SelectListItem()
                {
                    Value = a,
                    Text = bb
                };

                select.Add(n);
            }

            return select;
        }

        public List<Job> GetMainCategories()
        {
            return _context.Job.Where(j => j.IsActive && j.Name != "صاحب آرایشگاه").ToList();
        }

        public List<SelectListItem> GetPersonel(int id)
        {
            return _context.Admin.Where(a => a.IsActive == true && a.AdminRole != 1 && a.SubJobId == id).Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();
        }

        public List<SelectListItem> GetServices()
        {
            return _context.SubJob.Where(s => s.IsActive == true).Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();
        }

        public List<SubJob> GetSubCategories(int id)
        {
            return _context.SubJob.Where(s => s.IsActive && s.ParentId == id && s.Name != "صاحب آرایشگاه").ToList();
        }

        public string GetUserFullName(int id)
        {
            return _context.User.Find(id).FullName;
        }

        public List<SelectListItem> GetWorkingTimeDueToPersonel(int id, int dayId)
        {
            return _context.WorkingTime.Where(w => w.IsActive == true && w.AdminId == id && w.DayId == dayId && w.IsReserved == false).Select(w => new SelectListItem()
            {
                Value = w.Id.ToString(),
                Text = w.TimePeriodText
            }).ToList();
        }

        public void NoneActiveFormerDays(DateTime n)
        {

            List<Models.Entities.WorkingDays> ActiveDays = _context.WorkingDays.Where(w => w.IsActive).ToList();

            foreach (var Item in ActiveDays)
            {
                if (Item.Date <= n)
                {
                    Item.IsActive = false;

                    _context.Update(Item);
                    _context.SaveChanges();
                }
            }
        }

        public void NoneActiveFormerTimes(DateTime n, TimeSpan ts)
        {
            List<WorkingTime> ActiveTimes = _context.WorkingTime.Where(w => w.IsActive == true).ToList();

            foreach (var Item in ActiveTimes)
            {
                if (Item.DayDate < n)
                {
                    Item.IsActive = false;

                    _context.Update(Item);
                    _context.SaveChanges();
                }

                if (Item.DayDate == n && Item.StartTime < ts)
                {
                    Item.IsActive = false;

                    _context.Update(Item);
                    _context.SaveChanges();
                }
            }
        }
    }
}
