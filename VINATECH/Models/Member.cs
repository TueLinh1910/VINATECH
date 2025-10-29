using System.ComponentModel.DataAnnotations;

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

        // trạng thái: 0 = chờ duyệt, 1 = active, 2 = khóa
        public int Status { get; set; } = 0;
    }
}

