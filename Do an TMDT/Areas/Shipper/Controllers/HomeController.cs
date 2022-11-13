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

namespace Do_an_TMDT.Areas.Shipper.Controllers
{
    [Area("shipper")]
    public class HomeController : Controller
    {
        public INotyfService _notyfService { get; }
        private readonly WEBBANGIAYContext _context;
        public HomeController(WEBBANGIAYContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public async Task<IActionResult> Index(int? page, int id)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsDonHang = new List<DonHang>();
            lsDonHang = await _context.DonHangs
                    .Where(x => x.TinhTrang.Equals("Đang giao") && x.MaNguoiGiaoHang.Equals(id))
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

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "Email", donHang.MaNguoiDung);
            ViewData["MaNguoiGiaoHang"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "Email", donHang.MaNguoiGiaoHang);
            return View(donHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TinhTrang")] DonHang donHang)
        {
            var DonHang = _context.DonHangs.Find(id);
            DonHang.TinhTrang = donHang.TinhTrang;
            _context.Update(DonHang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.MaDonHang == id);
        }
    }
}
