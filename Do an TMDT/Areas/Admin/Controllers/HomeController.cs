using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using Do_an_CCNPMM.Models;
using Do_an_CCNPMM.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Do_an_CCNPMM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminLogin")]
    public class HomeController : Controller
    {
        public INotyfService _notyfService { get; }
        // GET: /<controller>/
        private readonly WEBBANGIAYContext _context;
        public HomeController(WEBBANGIAYContext context)
        {
            _context = context;
        }
        public IActionResult Index(string year="")
        {
            int selectedyear=-1;
            Int32.TryParse(year, out selectedyear);
            if (year == "" || selectedyear==-1)
            {
                year = DateTime.Now.Year.ToString();
                Int32.TryParse(year, out selectedyear); 
            }
            var allyear = _context.DonHangs.Select(m => new SelectListItem() { Text = m.NgayXuatDonHang.Value.Year.ToString(), Value = m.NgayXuatDonHang.Value.Year.ToString(), Selected = (m.NgayXuatDonHang.Value.Year == selectedyear) }).Distinct();
            ViewBag.allyear = allyear;
            ViewBag.TongDoanhThu = string.Format("{0:0,0} VND", ThongKeTongDoanhThu());
            ViewBag.TangTruong = 0;
            if (ThongKeDoanhThuTheoNam(selectedyear - 2) != 0)
            {
                ViewBag.TangTruong = string.Format("{0:0.00}", (ThongKeDoanhThuTheoNam(selectedyear - 1) - ThongKeDoanhThuTheoNam(selectedyear - 2)) / ThongKeDoanhThuTheoNam(selectedyear - 2) * 100);
            }


            ViewBag.SoDonHang = ThongKeTongDonHang();
            ViewBag.SoNguoiDung = ThongKeTongNguoiDung();
            ViewBag.DoanhThuTrungBinh = string.Format("{0:0,0}", ThongKeDoanhThuTheoNam(selectedyear) / 12);
            ViewBag.TongDoanhThuThang1 = Convert.ToInt32(ThongKeDoanhThuTheoThang(1, selectedyear));
            ViewBag.TongDoanhThuThang2 = Convert.ToInt32(ThongKeDoanhThuTheoThang(2, selectedyear));
            ViewBag.TongDoanhThuThang3 = Convert.ToInt32(ThongKeDoanhThuTheoThang(3, selectedyear));
            ViewBag.TongDoanhThuThang4 = Convert.ToInt32(ThongKeDoanhThuTheoThang(4, selectedyear));
            ViewBag.TongDoanhThuThang5 = Convert.ToInt32(ThongKeDoanhThuTheoThang(5, selectedyear));
            ViewBag.TongDoanhThuThang6 = Convert.ToInt32(ThongKeDoanhThuTheoThang(6, selectedyear));
            ViewBag.TongDoanhThuThang7 = Convert.ToInt32(ThongKeDoanhThuTheoThang(7, selectedyear));
            ViewBag.TongDoanhThuThang8 = Convert.ToInt32(ThongKeDoanhThuTheoThang(8, selectedyear));
            ViewBag.TongDoanhThuThang9 = Convert.ToInt32(ThongKeDoanhThuTheoThang(9, selectedyear));
            ViewBag.TongDoanhThuThang10 = Convert.ToInt32(ThongKeDoanhThuTheoThang(10, selectedyear));
            ViewBag.TongDoanhThuThang11 = Convert.ToInt32(ThongKeDoanhThuTheoThang(11, selectedyear));
            ViewBag.TongDoanhThuThang12 = Convert.ToInt32(ThongKeDoanhThuTheoThang(12, selectedyear));
            ViewBag.TongDoanhThuNamHienTai = string.Format("{0:0,0}", ThongKeDoanhThuTheoNam(selectedyear));
            ViewBag.Top5SanPham = Top5SanPham();
            List<string> AnhTop5SanPham = new List<string>();
            foreach (var item in Top5SanPham().ToList())
            {
                var obj = item.MatHangAnhs.First();
                AnhTop5SanPham.Add(obj.Anh);
            }
            ViewBag.AnhTop5SanPham = AnhTop5SanPham;

            return View();
        }
        public IActionResult Filtter(string year ="")
        {
            var url = $"/Admin/Home?year={year.Trim()}";
            if (year == "")
            {
                url = $"/Admin/Home";
            }
            return Json(new { status = "success", redirectUrl = url });
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
        public decimal ThongKeDoanhThuTheoThang(int Thang, int Nam)
        {
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
                return DoanhThu;
            }
            return 0;
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
