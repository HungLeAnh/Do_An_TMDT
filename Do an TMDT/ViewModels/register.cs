using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Do_an_CCNPMM.ViewModels
{
    public class register
    { 
            [Key]
            public int MaNguoiDung { get; set; }

            [Display(Name = "Họ và Tên")]
            [Required(ErrorMessage = "Vui lòng nhập Họ Tên")]
            public string TenNguoiDung { get; set; }

            [MaxLength(150)]
            [Required(ErrorMessage = "Vui lòng nhập Email")]
            [DataType(DataType.EmailAddress)]
            [Remote(action: "ValidateEmail", controller: "Accounts")]
            public string Email { get; set; }

            [MaxLength(11)]
            [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
            [Display(Name = "Điện thoại")]
            [DataType(DataType.PhoneNumber)]
            [Remote(action: "ValidatePhone", controller: "Accounts")]
            public string Sdt { get; set; }

            [Display(Name = "Mật khẩu")]
            [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
            [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
            public string MatKhauHash { get; set; }

            [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
            [Display(Name = "Nhập lại mật khẩu")]
            [Compare("MatKhauHash", ErrorMessage = "Nhập lại mật khẩu không đúng")]
            public string MaLoaiNguoiDung { get; set; }
            public string Salt { get; set; }
            public string TenDangNhap { get; set; }

    }

}

