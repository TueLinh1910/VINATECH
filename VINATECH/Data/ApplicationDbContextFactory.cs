using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace VINATECH.Data
{
    // Lớp này giúp EF Core tạo DbContext khi chạy lệnh Add-Migration hoặc Update-Database
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // ✅ Tự động tìm file appsettings.json trong project hiện tại
            var basePath = Directory.GetCurrentDirectory();

            // Nếu không tìm thấy, thử lùi 1 cấp thư mục (phòng khi chạy từ thư mục cha)
            if (!File.Exists(Path.Combine(basePath, "appsettings.json")))
            {
                basePath = Directory.GetParent(basePath).FullName;
            }

            // Đọc cấu hình từ appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Lấy chuỗi kết nối "DefaultConnection"
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Cấu hình DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Trả về đối tượng DbContext cho EF Core dùng
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}




