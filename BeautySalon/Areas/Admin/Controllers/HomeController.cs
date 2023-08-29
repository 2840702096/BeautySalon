using BeautySalon.Models.Context;
using BeautySalon.Models.Entities;
using BeautySalon.Models.Services.Interfaces;
using BeautySalon.Models.Tools;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BeautySalon.Areas.Admin.Controllers
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

        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }

        #region JobList

        [Area("Admin")]
        [Route("/Admin/JobList")]
        public IActionResult JobList()
        {
            List<Job> Jobs = _context.Job.ToList();

            return View(Jobs);
        }

        #region CreateJob

        [Area("Admin")]
        [Route("/Admin/CreateJob")]
        public IActionResult CreateJob()
        {
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/CreateJob")]
        [HttpPost]
        public IActionResult CreateJob(CreateJobViewModel job)
        {

            if (!ModelState.IsValid)
            {
                return View(job);
            }

            Job NewJob = new Job();
            NewJob.Name = job.JobName;
            NewJob.IsActive = true;

            string ImagePath = "\\Admin\\img\\JobImages\\";

            string ThumbPath = "\\Admin\\img\\JobThumbnail\\";

            ImageHelper i = new ImageHelper(_en);

            NewJob.Image = i.AddImage(job.ImageName, ImagePath, ThumbPath);

            _context.Job.Add(NewJob);
            _context.SaveChanges();

            return Redirect("/Admin/JobList");
        }

        #endregion

        #region EditJob

        [Area("Admin")]
        [Route("/Admin/EditJob/{id}")]
        public IActionResult EditJob(int id)
        {
            EditJobViewModel Job = _context.Job.Where(j => j.Id == id).Select(j => new EditJobViewModel
            {
                CurrentImage = j.Image,
                JobName = j.Name
            }).Single();

            return View(Job);
        }

        [Area("Admin")]
        [Route("/Admin/EditJob/{id}")]
        [HttpPost]
        public IActionResult EditJob(int id, EditJobViewModel updatedJob, string currentImage)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedJob);
            }

            Job CurrentJob = _context.Job.Find(id);

            CurrentJob.Name = updatedJob.JobName;

            string ImagePath = "\\Admin\\img\\JobImages\\";

            string ThumbPath = "\\Admin\\img\\JobThumbnail\\";

            ImageHelper i = new ImageHelper(_en);

            CurrentJob.Image = i.EditImage(updatedJob.ImageName, currentImage, ImagePath, ThumbPath);

            _context.Update(CurrentJob);
            _context.SaveChanges();

            List<SubJob> subJobs = _context.SubJob.Where(s => s.ParentId == id).ToList();

            if (subJobs != null)
            {
                foreach (var Item in subJobs)
                {
                    Item.ParentName = updatedJob.JobName;

                    _context.Update(Item);
                    _context.SaveChanges();
                }
            }

            return Redirect("/Admin/JobList");
        }

        #endregion

        #region Activation

        [Area("Admin")]
        [Route("/Admin/ActivateJob/{id}")]
        public IActionResult ActivateJob(int id)
        {
            Job Job = _context.Job.Find(id);
            Job.IsActive = true;

            _context.Update(Job);
            _context.SaveChanges();

            return Redirect("/Admin/JobList");
        }

        [Area("Admin")]
        [Route("/Admin/MakeJobNonActive/{id}")]
        public IActionResult MakeJobNonActive(int id)
        {
            Job Job = _context.Job.Find(id);
            Job.IsActive = false;

            _context.Update(Job);
            _context.SaveChanges();

            return Redirect("/Admin/JobList");
        }

        #endregion

        #region SubJob

        [Area("Admin")]
        [Route("/Admin/SubJobList/{id}")]
        public IActionResult SubJobList(int id, string title)
        {
            List<SubJob> SubJob = _context.SubJob.Where(s => s.ParentId == id).ToList();

            ViewBag.Id = id;
            ViewBag.Title = title;

            return View(SubJob);
        }

        #region Description

        [Area("Admin")]
        [Route("/Admin/SubJobDescription/{id}")]
        public IActionResult SubJobDescription(int id, string description)
        {
            ViewBag.ParentId = id;
            ViewBag.Description = description;

            return View();
        }

        #endregion

        #region CreateSubJob


        [Area("Admin")]
        [Route("/Admin/CreateSubJob/{id}")]
        public IActionResult CreateSubJob(int id, string title)
        {
            ViewBag.Id = id;
            ViewBag.Title = title;
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/CreateSubJob/{id}")]
        [HttpPost]
        public IActionResult CreateSubJob(int id, string title, CreateSubJobViewModel subJob)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                ViewBag.Title = title;
                return View(subJob);
            }

            SubJob NewSubJob = new SubJob();

            NewSubJob.Name = subJob.SubJobName;
            NewSubJob.Score = int.Parse(subJob.Score);
            NewSubJob.ParentId = id;
            NewSubJob.ParentName = title;
            NewSubJob.Price = subJob.Price;
            NewSubJob.ReservationCost = subJob.ReservationCost;
            NewSubJob.Description = subJob.Description;
            NewSubJob.IsActive = true;

            string ImagePath = "\\Admin\\img\\SubJobImages\\";

            string ThumbPath = "\\Admin\\img\\SubJobThumbnail\\";

            ImageHelper i = new ImageHelper(_en);

            NewSubJob.Image = i.AddImage(subJob.ImageName, ImagePath, ThumbPath);

            _context.Add(NewSubJob);
            _context.SaveChanges();

            return Redirect($"/Admin/SubJobList/{id}");
        }

        #endregion

        #region EditSubJob

        [Area("Admin")]
        [Route("/Admin/EditSubJob/{id}")]
        public IActionResult EditSubJob(int id, int parentId)
        {
            ViewBag.Id = parentId;

            EditSubJobViewModel SubJob = _context.SubJob.Where(s => s.Id == id).Select(s => new EditSubJobViewModel
            {
                SubJobName = s.Name,
                Score = s.Score.ToString(),
                Price = s.Price,
                ReservationCost = s.ReservationCost,
                Description = s.Description,
                CurrentImage = s.Image
            }).Single();

            return View(SubJob);
        }

        [Area("Admin")]
        [Route("/Admin/EditSubJob/{id}")]
        [HttpPost]
        public IActionResult EditSubJob(int id, EditSubJobViewModel updatedSubJob, string currentImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(updatedSubJob);
            }

            SubJob SubJob = _context.SubJob.Find(id);

            SubJob.Name = updatedSubJob.SubJobName;
            SubJob.Score = int.Parse(updatedSubJob.Score);
            SubJob.Price = updatedSubJob.Price;
            SubJob.ReservationCost = updatedSubJob.ReservationCost;
            SubJob.Description = updatedSubJob.Description;

            string ImagePath = "\\Admin\\img\\SubJobImages\\";

            string ThumbPath = "\\Admin\\img\\SubJobThumbnail\\";

            ImageHelper i = new ImageHelper(_en);

            SubJob.Image = i.EditImage(updatedSubJob.ImageName, currentImage, ImagePath, ThumbPath);

            _context.Update(SubJob);
            _context.SaveChanges();

            return Redirect($"/Admin/SubJobList/{SubJob.ParentId}");
        }

        #endregion

        #region Activation

        [Area("Admin")]
        [Route("/Admin/ActivateSubJob/{id}")]
        public IActionResult ActivateSubJob(int id, int parentId)
        {
            SubJob SubJob = _context.SubJob.Find(id);
            SubJob.IsActive = true;

            _context.Update(SubJob);
            _context.SaveChanges();

            return Redirect($"/Admin/SubJobList/{parentId}");
        }

        [Area("Admin")]
        [Route("/Admin/MakeSubJobNonActive/{id}")]
        public IActionResult MakeSubJobNonActive(int id, int parentId)
        {
            SubJob SubJob = _context.SubJob.Find(id);
            SubJob.IsActive = false;

            _context.Update(SubJob);
            _context.SaveChanges();

            return Redirect($"/Admin/SubJobList/{parentId}");
        }

        #endregion

        #endregion

        #endregion

        #region Admin

        [Area("Admin")]
        [Route("/Admin/AdminPage")]
        public IActionResult AdminPage()
        {
            List<Models.Entities.Admin> Admins = _context.Admin.ToList();

            return View(Admins);
        }

        [Area("Admin")]
        [Route("/Admin/AdminSpecification/{id}")]
        public IActionResult AdminSpecification(int id)
        {
            return View(_context.Admin.Find(id));
        }

        #region CreateAdmin

        [Area("Admin")]
        [Route("/Admin/CreateAdmin")]
        public IActionResult CreateAdmin()
        {
            ViewBag.SubJob = _context.SubJob.ToList();
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/CreateAdmin")]
        [HttpPost]
        public IActionResult CreateAdmin(CreateAdminViewModel newAdmin)
        {
            if (!ModelState.IsValid)
            {
                return View(newAdmin);
            }

            string[] Category = newAdmin.SubJob.Split(",");

            Models.Entities.Admin NewAdmin = new Models.Entities.Admin();

            NewAdmin.Name = newAdmin.Name;
            NewAdmin.UserName = newAdmin.UserName;
            NewAdmin.Password = newAdmin.Password;
            NewAdmin.Percent = int.Parse(newAdmin.Percent);
            NewAdmin.Phone = newAdmin.Phone;
            NewAdmin.Address = newAdmin.Address;
            NewAdmin.Description = newAdmin.Description;
            NewAdmin.IsActive = true;
            NewAdmin.SubJobId = Convert.ToInt32(Category.AsQueryable().First());
            NewAdmin.SubJobName = Category.AsQueryable().Last();
            NewAdmin.AdminRole = 2;

            string ImagePath = "\\Admin\\img\\AdminImage\\";

            string ThumbPath = "\\Admin\\img\\AdminThumbnail\\";

            ImageHelper i = new ImageHelper(_en);

            NewAdmin.Image = i.AddImage(newAdmin.Image, ImagePath, ThumbPath);

            _context.Add(NewAdmin);
            _context.SaveChanges();

            return Redirect("/Admin/AdminPage");
        }

        #endregion

        #region EditAdmin

        [Area("Admin")]
        [Route("/Admin/EditAdmin/{id}")]
        public IActionResult EditAdmin(int id)
        {
            EditAdminViewModel Admin = _context.Admin.Where(a => a.Id == id).Select(a => new EditAdminViewModel
            {
                Name = a.Name,
                SubJobId = a.SubJobId,
                SubJobName = a.SubJobName,
                UserName = a.UserName,
                Password = a.Password,
                Percent = a.Percent.Value.ToString(),
                Phone = a.Phone,
                Address = a.Address,
                Description = a.Description,
                CurrentImage = a.Image
            }).Single();

            ViewBag.SubJob = _context.SubJob.Where(s => s.Id != Admin.SubJobId).ToList();

            return View(Admin);
        }

        [Area("Admin")]
        [Route("/Admin/EditAdmin/{id}")]
        [HttpPost]
        public IActionResult EditAdmin(int id, EditAdminViewModel updatedAdmin, string currentImage)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedAdmin);
            }

            string[] SubJob = updatedAdmin.SubJob.Split(",");

            Models.Entities.Admin CurrentAdmin = _context.Admin.Find(id);

            CurrentAdmin.Name = updatedAdmin.Name;
            CurrentAdmin.UserName = updatedAdmin.UserName;
            CurrentAdmin.Address = updatedAdmin.Address;
            CurrentAdmin.Description = updatedAdmin.Description;
            CurrentAdmin.Percent = int.Parse(updatedAdmin.Percent);
            CurrentAdmin.Phone = updatedAdmin.Phone;
            CurrentAdmin.SubJobId = Convert.ToInt32(SubJob.AsQueryable().First());
            CurrentAdmin.SubJobName = SubJob.AsQueryable().Last();
            CurrentAdmin.Password = updatedAdmin.Password;

            string ImagePath = "\\Admin\\img\\AdminImage\\";

            string ThumbPath = "\\Admin\\img\\AdminThumbnail\\";

            ImageHelper i = new ImageHelper(_en);

            CurrentAdmin.Image = i.EditImage(updatedAdmin.Image, currentImage, ImagePath, ThumbPath);

            _context.Update(CurrentAdmin);
            _context.SaveChanges();

            return Redirect("/Admin/AdminPage");
        }

        #endregion

        #region Activation

        [Area("Admin")]
        [Route("/Admin/ActivateAdmin/{id}")]
        public IActionResult ActivateAdmin(int id)
        {
            Models.Entities.Admin Admin = _context.Admin.Find(id);
            Admin.IsActive = true;

            _context.Update(Admin);
            _context.SaveChanges();

            return Redirect($"/Admin/AdminSpecification/{id}");
        }

        [Area("Admin")]
        [Route("/Admin/MakeAdminNonActive/{id}")]
        public IActionResult MakeAdminNonActive(int id)
        {
            Models.Entities.Admin Admin = _context.Admin.Find(id);
            Admin.IsActive = false;

            _context.Update(Admin);
            _context.SaveChanges();

            return Redirect($"/Admin/AdminSpecification/{id}");
        }

        #endregion

        #endregion

        #region WorkingDays

        [Area("Admin")]
        [Route("/Admin/WorkingDays")]
        public IActionResult WorkingDays()
        {
            List<Models.Entities.WorkingDays> Day = _context.WorkingDays.ToList();

            DateTime N = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            ViewBag.Now = N;

            _helpingService.NoneActiveFormerDays(N);

            return View(Day);
        }

        #region CreateWorkingDay

        [Area("Admin")]
        [Route("/Admin/CreateWorkingDay")]
        public IActionResult CreateWorkingDay()
        {
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/CreateWorkingDay")]
        [HttpPost]
        public IActionResult CreateWorkingDay(CreateWorkingDayViewModel day)
        {
            if (!ModelState.IsValid)
            {
                return View(day);
            }

            if (_context.WorkingDays.Any(w => w.IsActive == true && w.Date == PersianDate.ToGeorgianDateTime(day.Date)))
            {
                ViewBag.TheExistDay = true;

                return View(day);
            }

            DateTime N = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var i = PersianDate.ToGeorgianDateTime(day.Date);
            if (i < N)
            {
                ViewBag.TodayOfADayBeforeToday = true;

                return View(day);
            }

            Models.Entities.WorkingDays WDay = new WorkingDays();

            WDay.Name = day.Name;
            WDay.Date = PersianDate.ToGeorgianDateTime(day.Date);
            WDay.StartTime = day.StartTime;
            WDay.EndTime = day.EndTime;
            WDay.IsActive = true;

            _context.Add(WDay);
            _context.SaveChanges();

            return Redirect("/Admin/WorkingDays");
        }

        #endregion

        #region EditWorkingDay

        [Area("Admin")]
        [Route("/Admin/EditWorkingDay/{id}")]
        public IActionResult EditWorkingDay(int id)
        {
            EditWorkingDayViewModel Day = _context.WorkingDays.Where(w => w.Id == id).Select(w => new EditWorkingDayViewModel
            {
                Name = w.Name,
                StartTime = w.StartTime,
                EndTime = w.EndTime
            }).Single();

            return View(Day);
        }

        [Area("Admin")]
        [Route("/Admin/EditWorkingDay/{id}")]
        [HttpPost]
        public IActionResult EditWorkingDay(int id, EditWorkingDayViewModel newDay)
        {
            if (!ModelState.IsValid)
            {
                return View(newDay);
            }

            if (_context.WorkingDays.Any(w => w.IsActive == true && w.Date == PersianDate.ToGeorgianDateTime(newDay.Date) && w.Id != id))
            {
                ViewBag.TheExistDay = true;

                return View(newDay);
            }

            if (PersianDate.ToGeorgianDateTime(newDay.Date) < DateTime.Now)
            {
                ViewBag.TodayOfADayBeforeToday = true;

                return View(newDay);
            }

            Models.Entities.WorkingDays CurrentDay = _context.WorkingDays.Find(id);

            CurrentDay.Name = newDay.Name;
            CurrentDay.Date = PersianDate.ToGeorgianDateTime(newDay.Date);
            CurrentDay.StartTime = newDay.StartTime;
            CurrentDay.EndTime = newDay.EndTime;

            _context.Update(CurrentDay);
            _context.SaveChanges();

            return Redirect("/Admin/WorkingDays");
        }

        #endregion

        #region Activation

        [Area("Admin")]
        [Route("/Admin/ActivateDay/{id}")]
        public IActionResult ActivateDay(int id)
        {
            Models.Entities.WorkingDays Day = _context.WorkingDays.Find(id);
            Day.IsActive = true;

            _context.Update(Day);
            _context.SaveChanges();

            return Redirect($"/Admin/WorkingDays");
        }

        [Area("Admin")]
        [Route("/Admin/MakeDayNonActive/{id}")]
        public IActionResult MakeDayNonActive(int id)
        {
            Models.Entities.WorkingDays Day = _context.WorkingDays.Find(id);
            Day.IsActive = false;

            _context.Update(Day);
            _context.SaveChanges();

            return Redirect($"/Admin/WorkingDays");
        }

        #endregion

        #region Delete

        [Area("Admin")]
        [Route("/Admin/DeleteWorkingDay/{id}")]
        public IActionResult DeleteWorkingDay(int id)
        {
            if (_context.WorkingTime.Any(w => w.DayId == id))
            {
                DateTime N = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                ViewBag.Now = N;

                List<Models.Entities.WorkingDays> Day = _context.WorkingDays.ToList();
                ViewBag.ValuedInWorkingTime = true;
                return View("WorkingDays", Day);
            }

            _context.Entry(_context.WorkingDays.Find(id)).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();

            return Redirect("/Admin/WorkingDays");
        }

        #endregion

        #endregion

        #region WorkingTimes

        [Area("Admin")]
        [Route("/Admin/WorkingTimes")]
        public IActionResult WorkingTimes()
        {
            List<WorkingTime> Time = _context.WorkingTime.ToList();

            DateTime N = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            ViewBag.Now = N;

            TimeSpan Ts = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            ViewBag.Time = Ts;

            _helpingService.NoneActiveFormerTimes(N, Ts);

            return View(Time);
        }

        #endregion

        #region Sliders

        [Area("Admin")]
        [Route("/Admin/Sliders")]
        public IActionResult Sliders()
        {
            List<Models.Entities.Sliders> Slider = _context.Sliders.ToList();

            return View(Slider);
        }

        #region CreateSlider

        [Area("Admin")]
        [Route("/Admin/CreateSlider")]
        public IActionResult CreateSlider()
        {
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/CreateSlider")]
        [HttpPost]
        public IActionResult CreateSlider(CreateSliderAndBannerViewModel slider)
        {
            if (!ModelState.IsValid)
            {
                return View(slider);
            }

            Models.Entities.Sliders NewSlider = new Sliders();

            NewSlider.Title = slider.Title;
            NewSlider.Tags = slider.Tags;
            NewSlider.Link = slider.Link;
            NewSlider.IsActive = true;
            NewSlider.CreateDate = DateTime.Now;

            ImageHelper i = new ImageHelper(_en);

            string ImagePath = "\\Admin\\img\\SliderImages\\";

            string ThumbPath = "\\Admin\\img\\SliderThumbNails\\";

            NewSlider.ImageName = i.AddImage(slider.ImageName, ImagePath, ThumbPath);

            _context.Add(NewSlider);
            _context.SaveChanges();

            return Redirect("/Admin/Sliders");
        }

        #endregion

        #region EditSlider

        [Area("Admin")]
        [Route("/Admin/EditSlider/{id}")]
        public IActionResult EditSlider(int id)
        {
            EditSliderAndBannerViewModel CurrentSlider = _context.Sliders.Where(s => s.Id == id).Select(s => new EditSliderAndBannerViewModel
            {
                Title = s.Title,
                Tags = s.Tags,
                Link = s.Link,
                CurrentImage = s.ImageName
            }).Single();

            return View(CurrentSlider);
        }

        [Area("Admin")]
        [Route("/Admin/EditSlider/{id}")]
        [HttpPost]
        public IActionResult EditSlider(int id, EditSliderAndBannerViewModel updatedSlider, string currentImage)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedSlider);
            }

            Models.Entities.Sliders NewSlider = _context.Sliders.Find(id);

            NewSlider.Title = updatedSlider.Title;
            NewSlider.Tags = updatedSlider.Tags;
            NewSlider.Link = updatedSlider.Link;

            ImageHelper i = new ImageHelper(_en);

            string ImagePath = "\\Admin\\img\\SliderImages\\";

            string ThumbPath = "\\Admin\\img\\SliderThumbNails\\";

            NewSlider.ImageName = i.EditImage(updatedSlider.ImageName, currentImage, ImagePath, ThumbPath);

            _context.Update(NewSlider);
            _context.SaveChanges();

            return Redirect("/Admin/Sliders");
        }

        #endregion

        #region Activation

        [Area("Admin")]
        [Route("/Admin/ActivateSlider/{id}")]
        public IActionResult ActivateSlider(int id)
        {
            Models.Entities.Sliders Slider = _context.Sliders.Find(id);
            Slider.IsActive = true;

            _context.Update(Slider);
            _context.SaveChanges();

            return Redirect($"/Admin/Sliders");
        }

        [Area("Admin")]
        [Route("/Admin/MakeSliderNonActive/{id}")]
        public IActionResult MakeSliderNonActive(int id)
        {
            Models.Entities.Sliders Slider = _context.Sliders.Find(id);
            Slider.IsActive = false;

            _context.Update(Slider);
            _context.SaveChanges();

            return Redirect($"/Admin/Sliders");
        }

        #endregion

        #region DeleteSlider

        [Area("Admin")]
        [Route("/Admin/DeleteSlider/{id}")]
        public IActionResult DeleteSlider(int id, string image)
        {
            Models.Entities.Sliders Slider = _context.Sliders.Find(id);

            _context.Entry(Slider).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();

            string Path = $"{_en.WebRootPath}\\Admin\\img\\SliderImages\\{image}";
            FileInfo file = new FileInfo(Path);
            if (file.Exists)
            {
                file.Delete();
            }

            string Path1 = $"{_en.WebRootPath}\\Admin\\img\\SliderThumbNails\\{image}";
            FileInfo file1 = new FileInfo(Path1);
            if (file1.Exists)
            {
                file1.Delete();
            }

            return Redirect("/Admin/Sliders");
        }

        #endregion

        #endregion

        #region Weblogs

        [Area("Admin")]
        [Route("/Admin/Weblogs")]
        public IActionResult Weblogs()
        {
            List<Models.Entities.Weblogs> Weblog = _context.Weblogs.ToList();
            return View(Weblog);
        }

        [Area("Admin")]
        [Route("/Admin/WeblogBody")]
        public IActionResult WeblogBody(string body)
        {
            ViewBag.Body = body;

            return View();
        }

        #region CreateWeblog

        [Area("Admin")]
        [Route("/Admin/CreateWeblog")]
        public IActionResult CreateWeblog()
        {
            ViewBag.Categories = _context.SubJob.ToList();

            return View();
        }

        [Area("Admin")]
        [Route("/Admin/CreateWeblog")]
        [HttpPost]
        public IActionResult CreateWeblog(CreateWeblogViewModel weblog)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.SubJob.ToList();
                return View(weblog);
            }

            string[] Category = weblog.SubJob.Split(",");

            Models.Entities.Weblogs NewWeblog = new Weblogs();

            NewWeblog.WeblogTitle = weblog.Title;
            NewWeblog.WeblogBody = weblog.WeblogBody;
            NewWeblog.CategoryId = Convert.ToInt32(Category.AsQueryable().First());
            NewWeblog.CategoryName = Category.AsQueryable().Last();
            NewWeblog.IsActive = true;
            NewWeblog.CreateDate = DateTime.Now;

            ImageHelper i = new ImageHelper(_en);

            string ImagePath = "\\Admin\\img\\WeblogImages\\";

            string ThumbPath = "\\Admin\\img\\WeblogThumbNail\\";

            NewWeblog.ImageName = i.AddImage(weblog.ImageName, ImagePath, ThumbPath);

            _context.Add(NewWeblog);
            _context.SaveChanges();

            return Redirect("/Admin/Weblogs");
        }

        #endregion

        #region EditWeblog

        [Area("Admin")]
        [Route("/Admin/EditWeblog/{id}")]
        public IActionResult EditWeblog(int id)
        {
            EditWeblogViewModel CurrentSlider = _context.Weblogs.Where(s => s.WeblogId == id).Select(s => new EditWeblogViewModel
            {
                Title = s.WeblogTitle,
                WeblogBody = s.WeblogBody,
                SubJobId = s.CategoryId,
                SubJobTitle = s.CategoryName,
                CurrentImage = s.ImageName
            }).Single();

            ViewBag.Categories = _context.SubJob.Where(s => s.Id != CurrentSlider.SubJobId).ToList();

            return View(CurrentSlider);
        }

        [Area("Admin")]
        [Route("/Admin/EditWeblog/{id}")]
        [HttpPost]
        public IActionResult EditWeblog(int id, EditWeblogViewModel updatedWeblog, string currentImage)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedWeblog);
            }

            string[] SubJob = updatedWeblog.SubJob.Split(",");

            Models.Entities.Weblogs CurrentWeblog = _context.Weblogs.Find(id);

            CurrentWeblog.WeblogTitle = updatedWeblog.Title;
            CurrentWeblog.WeblogBody = updatedWeblog.WeblogBody;
            CurrentWeblog.IsActive = true;
            CurrentWeblog.CategoryId = Convert.ToInt32(SubJob.AsQueryable().First());
            CurrentWeblog.CategoryName = SubJob.AsQueryable().Last();

            string ImagePath = "\\Admin\\img\\WeblogImages\\";

            string ThumbPath = "\\Admin\\img\\WeblogThumbNail\\";

            ImageHelper i = new ImageHelper(_en);

            CurrentWeblog.ImageName = i.EditImage(updatedWeblog.ImageName, currentImage, ImagePath, ThumbPath);

            _context.Update(CurrentWeblog);
            _context.SaveChanges();

            return Redirect("/Admin/Weblogs");
        }

        #endregion

        #region Activation

        [Area("Admin")]
        [Route("/Admin/ActivateWeblog/{id}")]
        public IActionResult ActivateWeblog(int id)
        {
            Models.Entities.Weblogs Weblog = _context.Weblogs.Find(id);
            Weblog.IsActive = true;

            _context.Update(Weblog);
            _context.SaveChanges();

            return Redirect($"/Admin/Weblogs");
        }

        [Area("Admin")]
        [Route("/Admin/MakeWeblogNonActive/{id}")]
        public IActionResult MakeWeblogNonActive(int id)
        {
            Models.Entities.Weblogs Weblog = _context.Weblogs.Find(id);
            Weblog.IsActive = false;

            _context.Update(Weblog);
            _context.SaveChanges();

            return Redirect($"/Admin/Weblogs");
        }

        #endregion

        #region DeleteWeblog

        [Area("Admin")]
        [Route("/Admin/DeleteWeblog/{id}")]
        public IActionResult DeleteWeblog(int id, string image)
        {
            Models.Entities.Weblogs Weblog = _context.Weblogs.Find(id);

            _context.Entry(Weblog).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();

            string Path = $"{_en.WebRootPath}\\Admin\\img\\WeblogImages\\{image}";
            FileInfo file = new FileInfo(Path);
            if (file.Exists)
            {
                file.Delete();
            }

            string Path1 = $"{_en.WebRootPath}\\Admin\\img\\WeblogThumbNail\\{image}";
            FileInfo file1 = new FileInfo(Path1);
            if (file1.Exists)
            {
                file1.Delete();
            }

            return Redirect("/Admin/Weblogs");
        }

        #endregion

        #endregion

        #region Users

        [Area("Admin")]
        [Route("/Admin/Users")]
        public IActionResult Users()
        {
            List<Models.Entities.User> Users = _context.User.ToList();

            return View(Users);
        }

        #endregion

        #region More

        #region Specifications

        [Area("Admin")]
        [Route("/Admin/SpecificationsOfBeautySalon")]
        public IActionResult SpecificationsOfBeautySalon()
        {
            List<Models.Entities.Specifications> Specifications = _context.Specifications.ToList();

            return View(Specifications);
        }

        [Area("Admin")]
        [Route("/Admin/SpecificationBody")]
        public IActionResult SpecificationBody(string body)
        {
            ViewBag.Body = body;
            return View();
        }

        #region EditSpecification

        [Area("Admin")]
        [Route("/Admin/EditSpecification/{id}")]
        public IActionResult EditSpecification(int id)
        {
            EditSpecificationViewModel Specification = _context.Specifications.Where(s => s.Id == id).Select(s => new EditSpecificationViewModel
            {
                Title = s.Title,
                SpecificationBody = s.Body,
                CurrentImage = s.Image
            }).Single();

            return View(Specification);
        }

        [Area("Admin")]
        [Route("/Admin/EditSpecification/{id}")]
        [HttpPost]
        public IActionResult EditSpecification(int id, EditSpecificationViewModel specifications, string currentImage)
        {
            Models.Entities.Specifications CurrentSpecification = _context.Specifications.Find(id);

            CurrentSpecification.Title = specifications.Title;
            CurrentSpecification.Body = specifications.SpecificationBody;

            string ImagePath = "\\Admin\\img\\SpecificationImages\\";

            string ThumbPath = "\\Admin\\img\\SpecificationTumbnails\\";

            ImageHelper i = new ImageHelper(_en);

            CurrentSpecification.Image = i.EditImage(specifications.ImageName, currentImage, ImagePath, ThumbPath);

            _context.Update(CurrentSpecification);
            _context.SaveChanges();

            return Redirect("/Admin/SpecificationsOfBeautySalon");
        }

        #endregion

        #region Activation

        [Area("Admin")]
        [Route("/Admin/ActivateSpecification/{id}")]
        public IActionResult ActivateSpecification(int id)
        {
            Models.Entities.Specifications Specification = _context.Specifications.Find(id);
            Specification.IsActive = true;

            _context.Update(Specification);
            _context.SaveChanges();

            return Redirect($"/Admin/SpecificationsOfBeautySalon");

            return Redirect($"/Admin/Weblogs");
        }

        [Area("Admin")]
        [Route("/Admin/MakeSpecificationNonActive/{id}")]
        public IActionResult MakeSpecificationNonActive(int id)
        {
            Models.Entities.Specifications Specification = _context.Specifications.Find(id);
            Specification.IsActive = false;

            _context.Update(Specification);
            _context.SaveChanges();

            return Redirect($"/Admin/SpecificationsOfBeautySalon");
        }

        #endregion

        #endregion

        #region Gallery

        [Area("Admin")]
        [Route("/Admin/Gallery")]
        public IActionResult Gallery()
        {
            List<Models.Entities.Gallery> galleries = _context.Gallery.ToList();

            return View(galleries);
        }

        #region AddImageToGallery

        [Area("Admin")]
        [Route("/Admin/AddImageToGallery")]
        public IActionResult AddImageToGallery()
        {
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/AddImageToGallery")]
        [HttpPost]
        public IActionResult AddImageToGallery(AddImageToGalleryViewModel addImage)
        {
            if (!ModelState.IsValid)
            {
                return View(addImage);
            }

            Models.Entities.Gallery NewImage = new Gallery();

            string ImagePath = "\\Admin\\img\\GalleryImages\\";

            string ThumbPath = "\\Admin\\img\\GalleryThumbNail\\";

            ImageHelper i = new ImageHelper(_en);

            NewImage.Image = i.AddImage(addImage.Image, ImagePath, ThumbPath);
            NewImage.IsActive = true;

            _context.Add(NewImage);
            _context.SaveChanges();

            return Redirect("/Admin/Gallery");
        }

        #endregion

        #region Activation

        [Area("Admin")]
        [Route("/Admin/ActivateImage/{id}")]
        public IActionResult ActivateImage(int id)
        {
            Models.Entities.Gallery Image = _context.Gallery.Find(id);
            Image.IsActive = true;

            _context.Update(Image);
            _context.SaveChanges();

            return Redirect($"/Admin/Gallery");
        }

        [Area("Admin")]
        [Route("/Admin/MakeImageNonActive/{id}")]
        public IActionResult MakeImageNonActive(int id)
        {
            Models.Entities.Gallery Image = _context.Gallery.Find(id);
            Image.IsActive = false;

            _context.Update(Image);
            _context.SaveChanges();

            return Redirect($"/Admin/Gallery");
        }

            #endregion

        #region DeleteImageFromGallery

            [Area("Admin")]
        [Route("/Admin/DeleteImageFromGallery/{id}")]
        public IActionResult DeleteImageFromGallery(int id, string currentImage)
        {
            string Path = $"{_en.WebRootPath}\\Admin\\\\img\\GalleryImages\\{currentImage}";
            FileInfo file = new FileInfo(Path);
            if (file.Exists)
            {
                file.Delete();
            }

            string Path1 = $"{_en.WebRootPath}\\Admin\\img\\GalleryThumbNail\\{currentImage}";
            FileInfo file1 = new FileInfo(Path1);
            if (file1.Exists)
            {
                file1.Delete();
            }

            _context.Entry(_context.Gallery.Find(id)).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

            _context.SaveChanges();

            return Redirect("/Admin/Gallery");
        }

        #endregion

        #endregion

        #endregion

        #region Reservations

        [Area("Admin")]
        [Route("/Admin/Reservations")]
        public IActionResult Reservations()
        {
            ViewBag.Model = _context.Reservations.AsNoTracking().ToList();
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/ReservationDescription/{id}")]
        public IActionResult ReservationDescription(int id)
        {
            ViewBag.Description = _context.Reservations.Find(id).Description;

            return View();
        }

        [Area("Admin")]
        [Route("/Admin/FinalPayment/{id}")]
        public IActionResult FinalPayment(int id)
        {
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/FinalPayment/{id}")]
        [HttpPost]
        public IActionResult FinalPayment(int id, int finalPayment)
        {
            if(finalPayment == 0)
            {
                ViewBag.Error = true;
                return View();
            }

            var Reservation = _context.Reservations.Find(id);

            Reservation.FinalPayment = finalPayment;

            _context.Update(Reservation);
            _context.SaveChanges();

            return View();
        }

        #endregion

    }
}
