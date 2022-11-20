using System;
using System.ComponentModel.DataAnnotations;

namespace Do_an_TMDT.Areas.Admin.ViewModels { 
    public class LoginViewModel
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage = ("Vui lòng nhập Email hoặc tên đăng nhập"))]
        [Display(Name = "Nhập Email hoặc tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
