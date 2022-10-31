using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class NhaCungCap
    {
        public NhaCungCap()
        {
            MatHangs = new HashSet<MatHang>();
        }

        public int MaNhaCungCap { get; set; }
        [Required]
        public string TenNhaCungCap { get; set; }
        public string Std { get; set; }

        public virtual ICollection<MatHang> MatHangs { get; set; }
    }
}
