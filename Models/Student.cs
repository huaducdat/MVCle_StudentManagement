using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        [Required(ErrorMessage = "Vui long nhap ma sinh vien")]
        [StringLength(20)]
        [Display(Name = "Ma sinh vien")]
        public string StudentCode { get; set; } = "";

        [Required(ErrorMessage = "Vui long nhap ho & ten")]
        [StringLength(100)]
        [Display(Name = "Ho & ten")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage ="Vui long chon ngay sinh")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngay sinh")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100)]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; } = "";
    }
}
