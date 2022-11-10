using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class GioHang
    {
        public GioHang()
        {
            ChiTietGioHangs = new HashSet<ChiTietGioHang>();
        }

        public int MaGioHang { get; set; }
        public int MaNguoiDung { get; set; }

        public virtual NguoiDung MaNguoiDungNavigation { get; set; }
        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }
    }
}
