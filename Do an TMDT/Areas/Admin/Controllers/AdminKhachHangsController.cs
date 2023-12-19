using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_CCNPMM.Models;
using PagedList.Core;
using Microsoft.AspNetCore.Authorization;


namespace Do_an_CCNPMM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminLogin")]

    public class AdminKhachHangsController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public AdminKhachHangsController(WEBBANGIAYContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminKhachHang
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsCustomers = _context.NguoiDungs
                .Where(x => 
                        x.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("khách hàng") || 
                        x.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("người dùng") ||
                        x.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.ToLower().Equals("user")
                        )
                .AsNoTracking()
                .OrderByDescending(x => x.MaNguoiDung)
                .Include(n => n.NguoiDungDiaChis);
            PagedList<NguoiDung> models = new PagedList<NguoiDung>(lsCustomers, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;

            ViewData["LoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "TenLoaiNguoiDung");
            return View(models);
        }

        // GET: Admin/AdminKhachHang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs
                .Include(n => n.MaLoaiNguoiDungNavigation)
                .FirstOrDefaultAsync(m => m.MaNguoiDung == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        // GET: Admin/AdminKhachHang/Create
        public IActionResult Create()
        {
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung");
            return View();
        }

        // POST: Admin/AdminKhachHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nguoiDung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
            return View(nguoiDung);
        }

        // GET: Admin/AdminKhachHang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
            return View(nguoiDung);
        }

        // POST: Admin/AdminKhachHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {
            if (id != nguoiDung.MaNguoiDung)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nguoiDung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguoiDungExists(nguoiDung.MaNguoiDung))
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
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
            return View(nguoiDung);
        }

        // GET: Admin/AdminKhachHang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs
                .Include(n => n.MaLoaiNguoiDungNavigation)
                .FirstOrDefaultAsync(m => m.MaNguoiDung == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        // POST: Admin/AdminKhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donhang = _context.DonHangs.Where(x => x.MaNguoiDung == id).ToList();
            foreach (var item in donhang)
            {
                item.MaNguoiDung = null;
            }
            await _context.SaveChangesAsync();

            var nguoi_DC = _context.NguoiDungDiaChis.Where(x => x.MaNguoiDung == id).ToList();
            foreach (var item in nguoi_DC)
            {
                _context.NguoiDungDiaChis.Remove(item);
            }
            await _context.SaveChangesAsync();

            var giohang = _context.GioHangs.Where(x => x.MaNguoiDung == id).ToList();
            var Tcgiohang = _context.ChiTietGioHangs.Where(x => x.MaGioHang == giohang[0].MaGioHang).ToList();
            foreach (var item in Tcgiohang)
            {
                _context.ChiTietGioHangs.Remove(item);
            }
          
            foreach (var item in giohang)
            {
                _context.GioHangs.Remove(item);
            }
            await _context.SaveChangesAsync();

            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            _context.NguoiDungs.Remove(nguoiDung);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NguoiDungExists(int id)
        {
            return _context.NguoiDungs.Any(e => e.MaNguoiDung == id);
        }
    }
}
