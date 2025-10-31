using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VINATECH.Data;

namespace VINATECH.Controllers
{
    public class PublicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trang hiển thị toàn bộ thành viên
        public async Task<IActionResult> Members()
        {
            var members = await _context.Members
                .Where(m => m.Status == 1) // chỉ lấy thành viên active
                .ToListAsync();

            return View(members);
        }
    }
}

