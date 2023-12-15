using Do_an_CCNPMM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Do_an_CCNPMM.ViewModels
{
    public class NguoiDungVm
    {
        public NguoiDung Ng { get; set; }
        public List<LoaiNguoiDung> LoaiNguoiDung { get; set; }
        public List<NguoiDungDiaChi> nguoiDungDiaChi { get; set; }
    }
}
