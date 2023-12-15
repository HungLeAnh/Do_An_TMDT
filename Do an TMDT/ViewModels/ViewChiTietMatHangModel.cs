using Do_an_CCNPMM.Models;
using System.Collections.Generic;
using System.Linq;

namespace Do_an_CCNPMM.ViewModels
{
    public class ViewChiTietMatHangModel
    {
        public MatHang MatHangs { get; set; }

        public string GetTenThuongHieu(int maThuongHieu)
        {
            string tenThuongHieu=string.Empty;
            var _context = new WEBBANGIAYContext();
            var _thuongHieu = (from s in _context.ThuongHieus where s.MaThuongHieu == maThuongHieu select s).ToList();
            if(_thuongHieu.Count > 0)
            {
                tenThuongHieu = _thuongHieu[0].TenThuongHieu;
            }
            return tenThuongHieu;
        }
    }
}
