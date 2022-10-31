using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class MatHangAnh
    {
        public int MaAnh { get; set; }
        public int MaMatHang { get; set; }
        [Required]
        public string Anh { get; set; }

        public virtual MatHang MaMatHangNavigation { get; set; }
    }
}
