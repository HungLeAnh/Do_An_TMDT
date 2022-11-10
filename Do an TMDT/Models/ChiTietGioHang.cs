using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class ChiTietGioHang
    {
        public int MaGioHang { get; set; }
        public int MaMatHang { get; set; }
        public int SoLuong { get; set; }
        public int Gia { get; set; }

        public virtual GioHang MaGioHangNavigation { get; set; }
        public virtual MatHang MaMatHangNavigation { get; set; }
    }
}
