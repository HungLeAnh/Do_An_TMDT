using Do_an_TMDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Do_an_TMDT.ViewModels
{
    public class MatHangHome
    {
        public MatHang listSPs{get;set;}
        public List<ThuongHieu> thuonghieu { get; set; }
        public List<MatHangAnh> MatHangAnhs { get; set; }

    }
}
