﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Do_an_CCNPMM.ViewModels
{
    public class LoginVM
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage = ("Vui lòng nhập Email"))]
        [Display(Name = "Địa chỉ Email")]
        [EmailAddress(ErrorMessage = "Sai định dạng Email")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        public string MatKhauHash { get; set; }
    }
}
