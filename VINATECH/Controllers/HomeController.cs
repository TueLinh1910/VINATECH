using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VINATECH.Data;

namespace VINATECH.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Trang chủ hiển thị bài viết mới nhất
        public IActionResult Index(int? categoryId)
        {
            var articles = _db.Articles
                .Include(a => a.Category)
                .OrderByDescending(a => a.PublishDate)
                .AsQueryable();

            // Nếu có chọn danh mục thì lọc
            if (categoryId.HasValue)
            {
                articles = articles.Where(a => a.CategoryId == categoryId.Value);
            }

            ViewBag.Categories = _db.Categories.ToList();
            return View(articles.ToList());
        }

        public IActionResult About()
        {
            ViewData["Title"] = "Giới thiệu VINATECH";
            return View();
        }

    }
}
