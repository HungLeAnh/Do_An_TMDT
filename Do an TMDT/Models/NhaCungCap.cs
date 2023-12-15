using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_CCNPMM.Models
{
    public partial class NhaCungCap
    {
        public NhaCungCap()
        {
            MatHangs = new HashSet<MatHang>();
        }

        public int MaNhaCungCap { get; set; }
        public string TenNhaCungCap { get; set; }
        public string Std { get; set; }

        public virtual ICollection<MatHang> MatHangs { get; set; }
    }
}
