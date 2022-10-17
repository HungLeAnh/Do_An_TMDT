using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class LoaiNguoiDung
    {
        public LoaiNguoiDung()
        {
            NguoiDungs = new HashSet<NguoiDung>();
        }

        public string MaLoaiNguoiDung { get; set; }
        public string TenLoaiNguoiDung { get; set; }

        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
    }
}
