using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_CCNPMM.Models
{
    public partial class NguoiDungDiaChi
    {
        public int MaDiaChi { get; set; }
        public int MaNguoiDung { get; set; }
        public string DiaChi { get; set; }

        public virtual NguoiDung MaNguoiDungNavigation { get; set; }
    }
}
