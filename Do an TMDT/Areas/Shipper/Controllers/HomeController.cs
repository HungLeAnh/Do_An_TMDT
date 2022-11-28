using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Do_an_TMDT.ViewModels;
using Do_an_TMDT.Helpper;
using Do_an_TMDT.Extension;
using MimeKit;
using MailKit.Net.Smtp;
using AspNetCoreHero.ToastNotification.Abstractions;
using PagedList.Core;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;

namespace Do_an_TMDT.Areas.Shipper.Controllers
{
    [Area("shipper")]
    [Authorize(AuthenticationSchemes = "ShipperLogin")]

    public class HomeController : Controller
    {
        public INotyfService _notyfService { get; }
        private readonly WEBBANGIAYContext _context;
        public HomeController(WEBBANGIAYContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public async Task<IActionResult> Index(int? page)
        {
            var manguoidung = Int32.Parse(User.Claims.First().Value);
            var nguoiDung = _context.NguoiDungs.Find(manguoidung);

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsDonHang = new List<DonHang>();
            lsDonHang = await _context.DonHangs
                    .Where(x => x.TinhTrang.Equals("Đang giao") && x.MaNguoiGiaoHang.Equals(nguoiDung.MaNguoiDung))
                    .Include(d => d.MaNguoiDungNavigation)
                    .Include(d => d.MaNguoiGiaoHangNavigation).ToListAsync();
            PagedList<DonHang> model = new PagedList<DonHang>(lsDonHang.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            string tinhTrang = _context.DonHangs.Select(n => n.TinhTrang).ToString();
            ViewBag.CurrentTinhTrang = tinhTrang;
            var lsTinhTrang = _context.DonHangs.Distinct().Select(m => new SelectListItem() { Text = m.TinhTrang, Value = m.TinhTrang, Selected = (m.TinhTrang == tinhTrang ? true : false) });
            ViewBag.TrangThai = lsTinhTrang;
            return View(model);

        }
        public IActionResult Filtter(string trangthai = "")
        {
            var url = $"/Shipper/Home?TinhTrang={trangthai.Trim()}";
            if (trangthai == "")
            {
                url = $"/Shipper/Home";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
      
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.MaNguoiDungNavigation)
                .Include(d => d.MaNguoiGiaoHangNavigation)
                .FirstOrDefaultAsync(m => m.MaDonHang == id);
            if (donHang == null)
            {
                return NotFound();
            }
            ViewBag.tinhtrang = donHang.TinhTrang;
            HttpContext.Session.SetInt32("MaDH", (int)id);
            ViewData["MaNguoiGiaoHang"] = new SelectList(_context.NguoiDungs
                                                        .Where(m => m.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("người giao hàng") ||
                                                               m.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("shipper")),
                                                     "MaNguoiDung", "MaNguoiDung");
            ViewBag.donhang = donHang;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit()
        {
            int id = (int)HttpContext.Session.GetInt32("MaDH");


            var donhang = _context.DonHangs.Where(x => x.MaDonHang == id).FirstOrDefault();
            ViewBag.tinhtrang = donhang.TinhTrang;
            if (donhang.TinhTrang == "Đang giao")
            {

                donhang.TinhTrang = "Đơn hàng sẽ được giao trong hôm nay";
                donhang.NgayDuKien = DateTime.Now;
                _context.Update(donhang);
                await _context.SaveChangesAsync();


                ViewBag.sus = "Đã cập nhật thành công";

                var listsp_donhang = _context.ChiTietDonHangs.Where(x => x.MaDonHang == id).ToList();
                var donHang = await _context.DonHangs
                  .Include(d => d.MaNguoiDungNavigation)
                  .Include(d => d.MaNguoiGiaoHangNavigation)
                  .FirstOrDefaultAsync(m => m.MaDonHang == id);
                if (donHang == null)
                {
                    return NotFound();
                }
                ViewBag.tinhtrang = donHang.TinhTrang;

                ViewData["MaNguoiGiaoHang"] = new SelectList(_context.NguoiDungs
                                                            .Where(m => m.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("người giao hàng") ||
                                                                   m.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("shipper")),
                                                         "MaNguoiDung", "MaNguoiDung");
                ViewBag.donhang = donHang;
                return View();
            }
            else if (donhang.TinhTrang == "Đơn hàng sẽ được giao trong hôm nay")
            {

                donhang.TinhTrang = "Đã giao";
                _context.Update(donhang);
                await _context.SaveChangesAsync();


                ViewBag.sus = "Đã giao thành công";

                var listsp_donhang = _context.ChiTietDonHangs.Where(x => x.MaDonHang == id).ToList();
                var donHang = await _context.DonHangs
                  .Include(d => d.MaNguoiDungNavigation)
                  .Include(d => d.MaNguoiGiaoHangNavigation)
                  .FirstOrDefaultAsync(m => m.MaDonHang == id);
                if (donHang == null)
                {
                    return NotFound();
                }
                ViewBag.tinhtrang = donHang.TinhTrang;

                ViewData["MaNguoiGiaoHang"] = new SelectList(_context.NguoiDungs
                                                            .Where(m => m.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("người giao hàng") ||
                                                                   m.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("shipper")),
                                                         "MaNguoiDung", "MaNguoiDung");
                ViewBag.donhang = donHang;
                return View();
            }
            else
            {
                var listsp_donhang = _context.ChiTietDonHangs.Where(x => x.MaDonHang == id).ToList();
                var donHang = await _context.DonHangs
                  .Include(d => d.MaNguoiDungNavigation)
                  .Include(d => d.MaNguoiGiaoHangNavigation)
                  .FirstOrDefaultAsync(m => m.MaDonHang == id);
                if (donHang == null)
                {
                    return NotFound();
                }
                ViewBag.tinhtrang = donHang.TinhTrang;

                ViewData["MaNguoiGiaoHang"] = new SelectList(_context.NguoiDungs
                                                            .Where(m => m.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("người giao hàng") ||
                                                                   m.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("shipper")),
                                                         "MaNguoiDung", "MaNguoiDung");
                ViewBag.donhang = donHang;
                return View();
            }
        }
            private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.MaDonHang == id);
        }
    }
}
