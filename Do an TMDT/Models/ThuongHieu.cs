using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class ThuongHieu
    {
        public ThuongHieu()
        {
            MatHangs = new HashSet<MatHang>();
        }

        public int MaThuongHieu { get; set; }
        [Required]
        public string TenThuongHieu { get; set; }

        public string Slug { get; set; }

        public virtual ICollection<MatHang> MatHangs { get; set; }
    }
}
