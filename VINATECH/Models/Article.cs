using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VINATECH.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Nội dung")]
        public string? Content { get; set; }

        [Display(Name = "Ngày đăng")]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; } = DateTime.Now;

        [Display(Name = "Ảnh minh họa")]
        public string? ImagePath { get; set; }

        // Liên kết đến danh mục
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}

