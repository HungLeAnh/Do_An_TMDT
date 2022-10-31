using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string TenDanhMuc { get; set; }
        [DefaultValue("")]
        public string Slug { get; set; }

        public virtual ICollection<MatHang> MatHangs { get; set; }
    }
}
