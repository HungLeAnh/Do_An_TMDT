using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class TheoDoi
    {
        public int MaTheoDoi { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaMatHang { get; set; }

        public virtual MatHang MaMatHangNavigation { get; set; }
        public virtual NguoiDung MaNguoiDungNavigation { get; set; }
    }
}
