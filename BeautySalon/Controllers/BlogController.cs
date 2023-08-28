using BeautySalon.Migrations;
using BeautySalon.Models.Context;
using BeautySalon.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace BeautySalon.Controllers
{
    public class BlogController : Controller
    {
        private SalonContext _context;

        public BlogController(SalonContext context)
        {
            _context = context;
        }

        [Route("/Blogs")]
        public IActionResult Index(int pageId = 1)
        {
            ViewBag.PageId = pageId;

            int Count = _context.Weblogs.Where(w => w.IsActive).Count();

            ViewBag.PageCount = Count / 15;

            int Take = 15;
            int Skip = (pageId  - 1) * Take;

            List<Weblogs> Blogs = _context.Weblogs.Where(w => w.IsActive).OrderByDescending(w=>w.CreateDate).Skip(Skip).Take(Take).ToList();

            ViewBag.SubCategories = _context.SubJob.Where(s=>s.IsActive && s.Name != "صاحب آرایشگاه").ToList();

            return View("BlogsPage", Blogs);
        }

        [Route("/Blogs/BringThemAcordingToCategory/{id}")]
        public IActionResult BringThemAcordingToCategory(int id)
        {
            List<Weblogs> FilteredWeblogs = _context.Weblogs.Where(w => w.CategoryId == id && w.IsActive).ToList();

            ViewBag.SubCategories = _context.SubJob.Where(s => s.IsActive && s.Name != "صاحب آرایشگاه").ToList();
            ViewBag.CategoryId = id;

            return View("BlogsPage", FilteredWeblogs);
        }

        [Route("/Blogs/SingleBlog/{id}")]
        public IActionResult SingleBlog(int id)
        {
            Weblogs Blog = _context.Weblogs.Find(id);
            Blog.Visit = Blog.Visit += 1;

            _context.Update(Blog);
            _context.SaveChanges();

            return View(Blog);
        }

        [Route("/Blogs/SearchBlogs")]
        public IActionResult SearchBlogs(string text, int pageId = 1)
        {
            if(text == null)
            {
                ViewBag.SubCategories = _context.SubJob.Where(s => s.IsActive).ToList();
                ViewBag.PageId = pageId;

                int count = _context.Weblogs.Where(w => w.IsActive).Count();

                ViewBag.PageCount = count / 15;

                int take = 15;
                int skip = (pageId - 1) * take;

                List<Weblogs> Blogs = _context.Weblogs.Where(w => w.IsActive).OrderByDescending(w => w.CreateDate).Skip(skip).Take(take).ToList();

                return View("BlogsPage", Blogs);
            }

            ViewBag.PageId = pageId;

            int Count = _context.Weblogs.Where(w => w.IsActive).Count();

            ViewBag.PageCount = Count / 15;

            int Take = 15;  
            int Skip = (pageId - 1) * Take;

            List<Weblogs> FilteredBlogs = _context.Weblogs.Where(w=>w.WeblogTitle.Contains(text) && w.IsActive).OrderByDescending(w => w.CreateDate).Skip(Skip).Take(Take).ToList();

            ViewBag.SubCategories = _context.SubJob.Where(s => s.IsActive).ToList();
            ViewBag.FilterText = text;

            return View("BlogsPage", FilteredBlogs);
        }
    }
}
