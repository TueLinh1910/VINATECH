using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VINATECH.Models;
using VINATECH.Data;

namespace VINATECH.Controllers
{
  public class AccountController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public AccountController(UserManager<ApplicationUser> userManager,
                             SignInManager<ApplicationUser> signInManager,
                             ApplicationDbContext context)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _context = context;
    }

    // --- ĐĂNG KÝ ---
    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(MemberRegisterViewModel model)
    {
      if (ModelState.IsValid)
      {
        var user = new ApplicationUser
        {
          UserName = model.FullName,
          Email = model.Email,
          Role = "Member"
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          // ✅ Lưu thông tin hội viên chờ duyệt
          var member = new Member
          {
            UserId = user.Id,
            FullName = model.FullName,
            Email = model.Email,
            Phone = model.Phone,
            Organization = model.Organization,
            Position = model.Position,
            Status = 0 // 0 = Chờ duyệt
          };

          _context.Members.Add(member);
          await _context.SaveChangesAsync();

          TempData["Success"] = "Đăng ký thành công! Vui lòng đợi admin duyệt trước khi đăng nhập.";
          return RedirectToAction("Login", "Account");
        }

        // Ghi lỗi từ Identity
        foreach (var error in result.Errors)
        {
          ModelState.AddModelError("", error.Description);
        }
      }

      return View(model);
    }

    // --- ĐĂNG NHẬP ---
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
      var user = await _userManager.FindByEmailAsync(email);
      if (user != null)
      {
        // 🔹 Kiểm tra xem có hồ sơ Member tương ứng không
        var member = _context.Members.FirstOrDefault(m => m.UserId == user.Id);

        // 🔸 Nếu là hội viên thì kiểm tra trạng thái duyệt
        if (member != null)
        {
          if (member.Status == 0)
          {
            ModelState.AddModelError("", "Tài khoản của bạn đang chờ duyệt. Vui lòng đợi admin xác nhận.");
            return View();
          }
          if (member.Status == -1)
          {
            ModelState.AddModelError("", "Tài khoản của bạn đã bị từ chối.");
            return View();
          }
        }

        // ✅ Đăng nhập nếu hợp lệ
        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        if (result.Succeeded)
        {
          if (user.Role == "Admin")
            return RedirectToAction("Index", "Admin");

          return RedirectToAction("Index", "Home");
        }
      }

      ModelState.AddModelError("", "Sai email hoặc mật khẩu!");
      return View();
    }

    // --- ĐĂNG XUẤT ---
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index", "Home");
    }
  }
}
