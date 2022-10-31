using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class KichCo
    {
        public KichCo()
        {
            MatHangs = new HashSet<MatHang>();
        }

        public int MaKichCo { get; set; }
        [Required]
        public double? KichCo1 { get; set; }

        public virtual ICollection<MatHang> MatHangs { get; set; }
    }
}
