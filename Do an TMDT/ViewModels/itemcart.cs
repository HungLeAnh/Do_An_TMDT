using Do_an_CCNPMM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Do_an_CCNPMM.ViewModels
{
    public class itemcart
    {
        public ChiTietGioHang CT_GH { get; set; }
        public MatHang SanPham { get; set; }
        public List<MatHangAnh> MatHangAnhs { get; set; }
        public long tong { get; set; }
    }
}
