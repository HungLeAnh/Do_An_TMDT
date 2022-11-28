using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class DanhMuc
    {
        public DanhMuc()
        {
            MatHangs = new HashSet<MatHang>();
        }

        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string Slug { get; set; }

        public virtual ICollection<MatHang> MatHangs { get; set; }
    }
}
