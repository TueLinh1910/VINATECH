using System.ComponentModel.DataAnnotations;

namespace VINATECH.Models
{
  public class MemberRegisterViewModel
  {
    [Required]
    [StringLength(200)]
    [Display(Name = "Họ và tên")]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Số điện thoại")]
    public string Phone { get; set; }

    [Display(Name = "Cơ quan / Tổ chức")]
    public string Organization { get; set; }

    [Display(Name = "Chức vụ")]
    public string Position { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
    [Display(Name = "Xác nhận mật khẩu")]
    public string ConfirmPassword { get; set; }
  }
}
