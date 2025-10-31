using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VINATECH.Data;
using VINATECH.Models;

namespace VINATECH.Controllers
{
    // 🟢 Chỉ Admin mới được truy cập
    [Authorize(Roles = "Admin")]
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env; // 🆕 để xử lý upload file

        public MembersController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var members = await _context.Members.ToListAsync();
            return View(members);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            if (ModelState.IsValid)
            {
                // 🖼 Upload ảnh nếu có
                if (member.AvatarFile != null)
                {
                    string uploadDir = Path.Combine(_env.WebRootPath, "uploads/members");
                    if (!Directory.Exists(uploadDir))
                        Directory.CreateDirectory(uploadDir);

                    string fileName = Guid.NewGuid() + Path.GetExtension(member.AvatarFile.FileName);
                    string filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await member.AvatarFile.CopyToAsync(stream);
                    }

                    member.AvatarPath = "/uploads/members/" + fileName;
                }

                _context.Add(member);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm thành viên thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();

            return View(member);
        }

        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Member member)
        {
            if (id != member.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existing = await _context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                if (existing == null) return NotFound();

                // Cập nhật thông tin
                existing.FullName = member.FullName;
                existing.Email = member.Email;
                existing.Phone = member.Phone;
                existing.Organization = member.Organization;
                existing.Position = member.Position;
                existing.Status = member.Status;

                // 🖼 Cập nhật ảnh mới (nếu có)
                if (member.AvatarFile != null)
                {
                    string uploadDir = Path.Combine(_env.WebRootPath, "uploads/members");
                    if (!Directory.Exists(uploadDir))
                        Directory.CreateDirectory(uploadDir);

                    string fileName = Guid.NewGuid() + Path.GetExtension(member.AvatarFile.FileName);
                    string filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await member.AvatarFile.CopyToAsync(stream);
                    }

                    existing.AvatarPath = "/uploads/members/" + fileName;
                }

                _context.Update(existing);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thành viên thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa thành viên thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}

