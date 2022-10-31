using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class LoaiNguoiDung
    {
        public LoaiNguoiDung()
        {
            NguoiDungs = new HashSet<NguoiDung>();
        }
        [Required]
        public string MaLoaiNguoiDung { get; set; }
        [Required]
        public string TenLoaiNguoiDung { get; set; }

        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
    }
}
