using System;
using System.Collections.Generic;

#nullable disable

namespace Do_an_CCNPMM.Models
{
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            DanhGia = new HashSet<DanhGium>();
        }

        public int MaDonHang { get; set; }
        public int? MaNguoiDung { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }
        public string TinhTrang { get; set; }
        public bool DaThanhToan { get; set; }
        public decimal TongTien { get; set; }
        public int? MaNguoiGiaoHang { get; set; }
        public DateTime? NgayXuatDonHang { get; set; }
        public string TenNguoiNhan { get; set; }
        public DateTime? NgayDuKien { get; set; }

        public virtual NguoiDung MaNguoiDungNavigation { get; set; }
        public virtual NguoiDung MaNguoiGiaoHangNavigation { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual ICollection<DanhGium> DanhGia { get; set; }
    }
}
