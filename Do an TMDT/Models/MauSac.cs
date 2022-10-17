using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class MauSac
    {
        public MauSac()
        {
            MatHangs = new HashSet<MatHang>();
        }

        public int MaMauSac { get; set; }
        public string TenMauSac { get; set; }

        public virtual ICollection<MatHang> MatHangs { get; set; }
    }
}
