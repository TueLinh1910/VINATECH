using Microsoft.AspNetCore.Mvc;
using VINATECH.Data;

namespace VINATECH.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var model = new
            {
                TotalArticles = _db.Articles.Count(),
                TotalCategories = _db.Categories.Count(),
                TotalMembers = _db.Members.Count()
            };

            ViewBag.TotalArticles = model.TotalArticles;
            ViewBag.TotalCategories = model.TotalCategories;
            ViewBag.TotalMembers = model.TotalMembers;

            return View();
        }
    }
}
