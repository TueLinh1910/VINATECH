using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VINATECH.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }
        public string Organization { get; set; }
        public string Position { get; set; }

        public int Status { get; set; } = 0;

        // 🆕 đường dẫn lưu ảnh đại diện
        public string? AvatarPath { get; set; }

        // 🆕 chỉ dùng để upload, không lưu trong DB
        [NotMapped]
        public IFormFile? AvatarFile { get; set; }
    }
}

