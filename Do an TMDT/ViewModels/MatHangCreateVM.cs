using Do_an_TMDT.Models;
using Microsoft.AspNetCore.Mvc;

namespace Do_an_TMDT.ViewModels
{
    //[Bind("MaMatHang,TenMatHang,GiaBan,DangDuocBan,SoSao,SoLuong,SoLuongDaBan,MoTa,DangDuocHienThi,MaNhaCungCap,MaThuongHieu,MaDanhMuc,MaKichCo,MaMauSac")]
    public class MatHangCreateVM
    {
        public MatHang MatHang { get; set; }
        public UploadFile UploadFile { get; set; }
    }
}
