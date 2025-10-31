using Microsoft.AspNetCore.Identity;

namespace VINATECH.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Thêm các trường bổ sung nếu cần
        public string? Role { get; set; }  // Vai trò (Admin / User)
    }
}
