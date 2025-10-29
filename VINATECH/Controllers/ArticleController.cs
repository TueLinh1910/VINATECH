using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VINATECH.Data;
using VINATECH.Models;

namespace VINATECH.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ArticleController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Hiển thị danh sách bài viết
        public async Task<IActionResult> Index()
        {
            var articles = await _db.Articles
                .Include(a => a.Category)
                .OrderByDescending(a => a.PublishDate)
                .ToListAsync();
            return View(articles);
        }

        // Xem chi tiết
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var article = await _db.Articles.Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (article == null) return NotFound();
            return View(article);
        }

        // Form tạo bài viết
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories.ToList();
            return View();
        }

        // Xử lý tạo bài viết
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                _db.Add(article);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _db.Categories.ToList();
            return View(article);
        }

        // Form sửa
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var article = await _db.Articles.FindAsync(id);
            if (article == null) return NotFound();
            ViewBag.Categories = _db.Categories.ToList();
            return View(article);
        }

        // Xử lý sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article)
        {
            if (id != article.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _db.Update(article);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _db.Categories.ToList();
            return View(article);
        }

        // Xóa
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var article = await _db.Articles.Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _db.Articles.FindAsync(id);
            if (article != null)
            {
                _db.Articles.Remove(article);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
