using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;
using Do_an_TMDT.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Do_an_TMDT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        public INotyfService _notyfService { get; }
        // GET: /<controller>/
        private readonly WEBBANGIAYContext _context;
        public HomeController(WEBBANGIAYContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            DateTime HienTai = DateTime.Now;
            ViewBag.TongDoanhThu = string.Format("{0:0,0}", ThongKeTongDoanhThu());
            ViewBag.TangTruong = 0;
            if (ThongKeDoanhThuTheoNam(HienTai.Year - 2) != 0)
            {
                ViewBag.TangTruong = string.Format("{0:0.00}", (ThongKeDoanhThuTheoNam(HienTai.Year - 1) - ThongKeDoanhThuTheoNam(HienTai.Year - 2)) / ThongKeDoanhThuTheoNam(HienTai.Year - 2) * 100);
            }
            ViewBag.SoDonHang = ThongKeTongDonHang();
            ViewBag.SoNguoiDung = ThongKeTongNguoiDung();
            ViewBag.DoanhThuTrungBinh = string.Format("{0:0,0.00}", ThongKeDoanhThuTheoNam(HienTai.Year) / HienTai.Month);
            ViewBag.TongDoanhThuThang1 = string.Format("{0:0,0}", ThongKeDoanhThuTheoThang(1, HienTai.Year));
            ViewBag.TongDoanhThuThang2 = string.Format("{0:0,0}", ThongKeDoanhThuTheoThang(2, HienTai.Year));
            ViewBag.TongDoanhThuThang3 = string.Format("{0:0,0}", ThongKeDoanhThuTheoThang(3, HienTai.Year));
            ViewBag.TongDoanhThuThang4 = string.Format("{0:0,0}", ThongKeDoanhThuTheoThang(4, HienTai.Year));
            ViewBag.TongDoanhThuThang5 = string.Format("{0:0,0}", ThongKeDoanhThuTheoThang(5, HienTai.Year));
            ViewBag.TongDoanhThuThang6 = string.Format("{0:0,0}", ThongKeDoanhThuTheoThang(6, HienTai.Year));
            ViewBag.TongDoanhThuThang7 = string.Format("{0:0,0}", ThongKeDoanhThuTheoThang(7, HienTai.Year));
            ViewBag.TongDoanhThuThang8 = string.Format("{0:0,0}", ThongKeDoanhThuTheoThang(8, HienTai.Year));
            ViewBag.TongDoanhThuThang9 = string.Format("{0:0,0}", ThongKeDoanhThuTheoThang(9, HienTai.Year));
            ViewBag.TongDoanhThuThang10 = string.Format("{0:0,0}", ThongKeDoanhThuTheoThang(10, HienTai.Year));
            ViewBag.TongDoanhThuThang11 = string.Format("{0:0,0}", ThongKeDoanhThuTheoThang(11, HienTai.Year));
            ViewBag.TongDoanhThuThang12 = string.Format("{0:0,0}", ThongKeDoanhThuTheoThang(12, HienTai.Year));
            ViewBag.TongDoanhThuNamHienTai = string.Format("{0:0,0}", ThongKeDoanhThuTheoNam(HienTai.Year));
            ViewBag.Top5SanPham = Top5SanPham();
            List<string> AnhTop5SanPham = new List<string>();
            foreach (var item in Top5SanPham().ToList())
            {
                var obj = item.MatHangAnhs.First();
                AnhTop5SanPham.Add(obj.Anh);
            }
            ViewBag.AnhTop5SanPham = AnhTop5SanPham;
            int idND = (int)HttpContext.Session.GetInt32("Ten");
            var NG = _context.NguoiDungs.Where(x => x.MaNguoiDung == idND).FirstOrDefault();
            ViewBag.NG = NG;
            return View();
        }
        public decimal ThongKeTongDoanhThu()
        {
            decimal TongDoanhThu = _context.MatHangs.Sum(n => n.SoLuongDaBan * (n.GiaBan - n.GiaNhap)).Value;
            return TongDoanhThu;
        }
        public decimal ThongKeDoanhThuTheoNam(int Nam)
        {
            if (_context.DonHangs.Where(n => n.NgayXuatDonHang.Value.Year == Nam).Count() > 0)
            {
                var lst = from s1 in (from s1 in _context.DonHangs.Where(n => n.NgayXuatDonHang.Value.Year == Nam)
                                      join s2 in _context.ChiTietDonHangs on s1.MaDonHang equals s2.MaDonHang
                                      select new
                                      {
                                          MaMatHang = s2.MaMatHang,
                                          TongTien = s1.TongTien,
                                          SoLuongDaBan = s2.SoLuong
                                      })
                          join s2 in _context.MatHangs on s1.MaMatHang equals s2.MaMatHang
                          select new
                          {
                              DoanhThu = s1.TongTien - s1.SoLuongDaBan * s2.GiaNhap
                          };
                decimal TongDoanhThu = decimal.Parse(lst.Sum(n => n.DoanhThu).ToString());
                return TongDoanhThu;
            }
            return 0;
        }
        public string ThongKeDoanhThuTheoThang(int Thang, int Nam)
        {
            DateTime HienTai = DateTime.Now;
            if (Thang > HienTai.Month)
            {
                return "Chưa có dữ liệu";
            }
            if (_context.DonHangs.Where(n => n.NgayXuatDonHang.Value.Year == Nam && n.NgayXuatDonHang.Value.Month == Thang).Count() > 0)
            {
                var lst = from s1 in (from s1 in _context.DonHangs.Where(n => n.NgayXuatDonHang.Value.Year == Nam && n.NgayXuatDonHang.Value.Month == Thang)
                                      join s2 in _context.ChiTietDonHangs on s1.MaDonHang equals s2.MaDonHang
                                      select new
                                      {
                                          MaMatHang = s2.MaMatHang,
                                          TongTien = s1.TongTien,
                                          SoLuongDaBan = s2.SoLuong
                                      })
                          join s2 in _context.MatHangs on s1.MaMatHang equals s2.MaMatHang
                          select new
                          {
                              DoanhThu = s1.TongTien - s1.SoLuongDaBan * s2.GiaNhap
                          };
                decimal DoanhThu = decimal.Parse(lst.Sum(n => n.DoanhThu).ToString());
                return DoanhThu.ToString();
            }
            return "0";
        }
        public int ThongKeTongDonHang()
        {
            int SoDonHang = _context.DonHangs.Count();
            return SoDonHang;
        }
        public int ThongKeTongNguoiDung()
        {
            int SoNguoiDung = _context.NguoiDungs.Count();
            return SoNguoiDung;
        }
        public List<MatHang> Top5SanPham()
        {
            var Top5SanPham = (from s1 in _context.MatHangs
                               orderby s1.SoLuongDaBan descending
                               select s1).Take(5).ToList();
            var AnhTop5SanPham = (from s1 in _context.MatHangAnhs
                                  join s2 in _context.MatHangs on s1.MaMatHang equals s2.MaMatHang
                                  orderby s2.SoLuongDaBan descending
                                  select s1).Take(5).ToList();
            int i = 0;
            foreach (var item in Top5SanPham)
            {
                item.MatHangAnhs.Add(AnhTop5SanPham[0]);
                i++;
            }
            return Top5SanPham;
        }
    }
}
