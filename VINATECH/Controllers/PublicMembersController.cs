using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VINATECH.Data;

namespace VINATECH.Controllers
{
    public class PublicMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trang hiển thị danh sách thành viên công khai
        public async Task<IActionResult> Index()
        {
            var members = await _context.Members
                .Where(m => m.Status == 1) // chỉ hiện thành viên đã duyệt (nếu có dùng cột Status)
                .ToListAsync();

            return View(members);
        }
    }
}
