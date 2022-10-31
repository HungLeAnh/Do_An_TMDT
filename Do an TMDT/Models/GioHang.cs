using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class GioHang
    {
        public int MaGioHang { get; set; }
        [Required]
        public int MaNguoiDung { get; set; }

        public virtual ChiTietGioHang MaGioHangNavigation { get; set; }
        public virtual NguoiDung MaNguoiDungNavigation { get; set; }
    }
}
