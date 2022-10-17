using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class ChiTietDonHang
    {
        public int MaChiTietDonHang { get; set; }
        public int MaDonHang { get; set; }
        public int MaMatHang { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }

        public virtual DonHang MaDonHangNavigation { get; set; }
        public virtual MatHang MaMatHangNavigation { get; set; }
    }
}
