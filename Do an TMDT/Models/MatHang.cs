using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class MatHang
    {
        public MatHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            ChiTietGioHangs = new HashSet<ChiTietGioHang>();
            DanhGia = new HashSet<DanhGia>();
            MatHangAnhs = new HashSet<MatHangAnh>();
            TheoDois = new HashSet<TheoDoi>();
        }

        public int MaMatHang { get; set; }
        public string TenMatHang { get; set; }
        public decimal GiaBan { get; set; }
        [DefaultValue(true)]
        public bool DangDuocBan { get; set; }
        [DefaultValue(0)]
        public double SoSao { get; set; }
        [Required]
        public int? SoLuong { get; set; }
        [DefaultValue(0)]
        public int? SoLuongDaBan { get; set; }
        [DefaultValue("")]
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
        public virtual ICollection<DanhGia> DanhGia { get; set; }
        public virtual ICollection<MatHangAnh> MatHangAnhs { get; set; }
        public virtual ICollection<TheoDoi> TheoDois { get; set; }
    }
}
