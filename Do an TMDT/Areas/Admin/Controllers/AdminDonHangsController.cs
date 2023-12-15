using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_CCNPMM.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList.Core;
using Microsoft.AspNetCore.Http;

namespace Do_an_CCNPMM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminDonHangsController : Controller
    {
        private readonly WEBBANGIAYContext _context;
        public INotyfService _notyfService { get; }

        public AdminDonHangsController(WEBBANGIAYContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;

        }

        // GET: Admin/AdminDonHangs
        public async Task<IActionResult> Index(int? page, string tinhTrang = "")
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsDonHang = new List<DonHang>();
            if (tinhTrang != "")
            {
                lsDonHang = await _context.DonHangs
                    .Where(x => x.TinhTrang == tinhTrang)
                    .Include(d => d.MaNguoiDungNavigation)
                    .Include(d => d.MaNguoiGiaoHangNavigation).ToListAsync();
            }
            else
            {
                lsDonHang = await _context.DonHangs
                    .Include(d => d.MaNguoiDungNavigation)
                    .Include(d => d.MaNguoiGiaoHangNavigation).ToListAsync();
            }

            PagedList<DonHang> model = new PagedList<DonHang>(lsDonHang.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentTinhTrang = tinhTrang;

            var lsTinhTrang = _context.DonHangs.Select(m => new SelectListItem() { Text = m.TinhTrang, Value = m.TinhTrang, Selected = (m.TinhTrang == tinhTrang) }).Distinct();
            ViewBag.TrangThai = lsTinhTrang;//new SelectList(lsDonHang.Select(m => m.TinhTrang).Distinct(), tinhTrang);
            return View(model);

        }
        public IActionResult Filtter(string trangthai = "")
        {
            var url = $"/Admin/AdminDonHangs?TinhTrang={trangthai.Trim()}";
            if (trangthai == "")
            {
                url = $"/Admin/AdminDonHangs";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        // GET: Admin/AdminDonHangs/Details/5
        public async Task<IActionResult> Details(int? id)
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
                                                     "MaNguoiDung", "TenNguoiDung");

            var ng = await _context.NguoiDungs.FindAsync(donHang.MaNguoiGiaoHang);
            if (ng != null)
            {
                ViewBag.Ten = ng.TenNguoiDung;
            }
            return View(donHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details()
        {

            int id = (int)HttpContext.Session.GetInt32("MaDH");


            var donhang = _context.DonHangs.Where(x => x.MaDonHang == id).FirstOrDefault();
            ViewBag.tinhtrang = donhang.TinhTrang;
            if (donhang.TinhTrang == "Đã xác nhận")
            {

                donhang.TinhTrang = "Đã đóng gói";
                _context.Update(donhang);
                await _context.SaveChangesAsync();
                 ViewBag.sus = "Đã xác nhận thành công";
            }

           

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
                                                     "MaNguoiDung", "TenNguoiDung");
            return View(donhang);
        }

        // GET: Admin/AdminDonHangs/Create
        public IActionResult Update()
        {
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs.Where(n => n.TinhTrang.ToLower().Equals("chưa giao")), "MaDonHang", "MaDonHang");
            ViewData["MaNguoiGiaoHang"] = new SelectList(_context.NguoiDungs
                                                        .Where(m => m.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("người giao hàng") ||
                                                               m.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("shipper")),
                                                     "MaNguoiDung", "TenNguoiDung");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind("MaDonHang, MaNguoiGiaoHang")] DonHang donHang,String id)
        {

           
                int MaDH = (int)HttpContext.Session.GetInt32("MaDH");
                var DonHang = _context.DonHangs.Find(MaDH);
                DonHang.MaNguoiGiaoHang = donHang.MaNguoiGiaoHang;
                DonHang.TinhTrang = "Đang giao";
                _context.Update(DonHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            
        }
        // POST: Admin/AdminDonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDonHang,MaNguoiDung,DiaChi,Sdt,TinhTrang,DaThanhToan,TongTien,MaNguoiGiaoHang,NgayXuatDonHang")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "Email", donHang.MaNguoiDung);
            ViewData["MaNguoiGiaoHang"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "Email", donHang.MaNguoiGiaoHang);
            return View(donHang);
        }

        // GET: Admin/AdminDonHangs/Edit/5
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

        // POST: Admin/AdminDonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDonHang,MaNguoiDung,DiaChi,Sdt,TinhTrang,DaThanhToan,TongTien,MaNguoiGiaoHang,NgayXuatDonHang")] DonHang donHang)
        {
            if (id != donHang.MaDonHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.MaDonHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "Email", donHang.MaNguoiDung);
            ViewData["MaNguoiGiaoHang"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "Email", donHang.MaNguoiGiaoHang);
            return View(donHang);
        }

        // GET: Admin/AdminDonHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(donHang);
        }

        // POST: Admin/AdminDonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var ChiTietDH = _context.ChiTietDonHangs.Where(x => x.MaDonHang == id).ToList();


   
                foreach (var item in ChiTietDH)
                {
                    _context.Remove(item);
                    await _context.SaveChangesAsync();
                }
            var donHang = await _context.DonHangs.FindAsync(id);
            _context.DonHangs.Remove(donHang);
            await _context.SaveChangesAsync();
           

            return RedirectToAction(nameof(Index));

        }

        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.MaDonHang == id);
        }
    }
}
