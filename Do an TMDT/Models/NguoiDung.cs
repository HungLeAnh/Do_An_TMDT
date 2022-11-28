using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_TMDT.Models
{
    public partial class NguoiDung
    {
        public NguoiDung()
        {
            DanhGia = new HashSet<DanhGium>();
            DonHangMaNguoiDungNavigations = new HashSet<DonHang>();
            DonHangMaNguoiGiaoHangNavigations = new HashSet<DonHang>();
            NguoiDungDiaChis = new HashSet<NguoiDungDiaChi>();
            TheoDois = new HashSet<TheoDoi>();
        }

        public int MaNguoiDung { get; set; }
        public string MaLoaiNguoiDung { get; set; }
        public string TenNguoiDung { get; set; }
        public string AnhDaiDien { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhauHash { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public string Sdt { get; set; }
        public decimal? ViDienTu { get; set; }

        public virtual LoaiNguoiDung MaLoaiNguoiDungNavigation { get; set; }
        public virtual GioHang GioHang { get; set; }
        public virtual ICollection<DanhGium> DanhGia { get; set; }
        public virtual ICollection<DonHang> DonHangMaNguoiDungNavigations { get; set; }
        public virtual ICollection<DonHang> DonHangMaNguoiGiaoHangNavigations { get; set; }
        public virtual ICollection<NguoiDungDiaChi> NguoiDungDiaChis { get; set; }
        public virtual ICollection<TheoDoi> TheoDois { get; set; }
    }
}
