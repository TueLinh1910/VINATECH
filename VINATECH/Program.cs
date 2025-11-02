using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VINATECH.Data;
using VINATECH.Models;


var builder = WebApplication.CreateBuilder(args);

// ⚙️ Kết nối database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ⚙️ Đăng ký Identity với ApplicationUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromSeconds(30); // Phiên hết hạn sau 30 phút
    options.SlidingExpiration = true; // Tự động gia hạn nếu người dùng còn hoạt động
    options.LoginPath = "/Account/Login"; // Trang login
    options.AccessDeniedPath = "/Account/AccessDenied"; // Trang không có quyền
});

builder.Services.AddControllersWithViews();


var app = builder.Build();

// 🟢 Tạo admin mặc định khi khởi động lần đầu
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Tạo vai trò "Admin" nếu chưa có
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    // Tạo tài khoản admin mặc định
    string adminEmail = "admin@vinatech.com";
    string adminPassword = "Admin@123";
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin == null)
    {
        var user = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            Role = "Admin",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}

// rest of pipeline...
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Xác thực
app.UseAuthorization();   // Phân quyền

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// app.MapRazorPages();

app.Run();
