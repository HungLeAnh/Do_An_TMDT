using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_CCNPMM.Models
{
    public partial class DanhGia
    {
        public int MaDanhGia { get; set; }
        public string NoiDung { get; set; }
        public double SoSao { get; set; }
        public int MaMatHang { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaDonHang { get; set; }

        public virtual DonHang MaDonHangNavigation { get; set; }
        public virtual MatHang MaMatHangNavigation { get; set; }
        public virtual NguoiDung MaNguoiDungNavigation { get; set; }
    }
}
