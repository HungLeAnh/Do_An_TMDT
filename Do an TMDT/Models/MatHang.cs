using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class MatHang
    {
        public MatHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            ChiTietGioHangs = new HashSet<ChiTietGioHang>();
            DanhGia = new HashSet<DanhGium>();
            MatHangAnhs = new HashSet<MatHangAnh>();
            TheoDois = new HashSet<TheoDoi>();
        }

        public int MaMatHang { get; set; }
        public string TenMatHang { get; set; }
        public decimal GiaBan { get; set; }
        public bool DangDuocBan { get; set; }
        public double SoSao { get; set; }
        public int? SoLuong { get; set; }
        public int? SoLuongDaBan { get; set; }
        public string MoTa { get; set; }
        public bool DangDuocHienThi { get; set; }
        public int MaNhaCungCap { get; set; }
        public int MaThuongHieu { get; set; }
        public int MaDanhMuc { get; set; }
        public int? MaKichCo { get; set; }
        public int? MaMauSac { get; set; }
        public int? GiaNhap { get; set; }

        public virtual DanhMuc MaDanhMucNavigation { get; set; }
        public virtual KichCo MaKichCoNavigation { get; set; }
        public virtual MauSac MaMauSacNavigation { get; set; }
        public virtual NhaCungCap MaNhaCungCapNavigation { get; set; }
        public virtual ThuongHieu MaThuongHieuNavigation { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public virtual ICollection<DanhGium> DanhGia { get; set; }
        public virtual ICollection<MatHangAnh> MatHangAnhs { get; set; }
        public virtual ICollection<TheoDoi> TheoDois { get; set; }
    }
}
