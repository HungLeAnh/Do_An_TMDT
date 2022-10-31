using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList.Core;

namespace Do_an_TMDT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMatHangsController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public AdminMatHangsController(WEBBANGIAYContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminMatHangs
        public ActionResult Index(int? page=1)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsMatHangs = _context.MatHangs
                .Include(m => m.MaDanhMucNavigation)
                .Include(m => m.MaKichCoNavigation)
                .Include(m => m.MaMauSacNavigation)
                .Include(m => m.MaNhaCungCapNavigation)
                .Include(m => m.MaThuongHieuNavigation)
                .Include(m => m.MatHangAnhs)
                .AsNoTracking()
                .OrderByDescending(x => x.MaMatHang);
            PagedList<MatHang> models = new PagedList<MatHang>(lsMatHangs, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewData[nameof(DanhMuc)] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            return View(models);
        }

        public IActionResult Filtter(int _MaDanhMuc = 0)
        {
            var url = $"/Admin/AdminMatHangs?MaMatHang={_MaDanhMuc}";
            if (_MaDanhMuc == 0)
            {
                url = $"/Admin/AdminMatHangs";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        // GET: Admin/AdminMatHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matHang = await _context.MatHangs
                .Include(m => m.MaDanhMucNavigation)
                .Include(m => m.MaKichCoNavigation)
                .Include(m => m.MaMauSacNavigation)
                .Include(m => m.MaNhaCungCapNavigation)
                .Include(m => m.MaThuongHieuNavigation)
                .FirstOrDefaultAsync(m => m.MaMatHang == id);
            if (matHang == null)
            {
                return NotFound();
            }

            return View(matHang);
        }

        // GET: Admin/AdminMatHangs/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewData["KichCo"] = new SelectList(_context.KichCos, "MaKichCo", "KichCo1");
            ViewData["MauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "TenMauSac");
            ViewData["NhaCungCap"] = new SelectList(_context.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap");
            ViewData["ThuongHieu"] = new SelectList(_context.ThuongHieus, "MaThuongHieu", "TenThuongHieu");
            return View();
        }

        // POST: Admin/AdminMatHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaMatHang,TenMatHang,GiaBan,DangDuocBan,SoSao,SoLuong,SoLuongDaBan,MoTa,DangDuocHienThi,MaNhaCungCap,MaThuongHieu,MaDanhMuc,MaKichCo,MaMauSac")] MatHang matHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(matHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "Slug", matHang.MaDanhMuc);
            ViewData["MaKichCo"] = new SelectList(_context.KichCos, "MaKichCo", "MaKichCo", matHang.MaKichCo);
            ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac", matHang.MaMauSac);
            ViewData["MaNhaCungCap"] = new SelectList(_context.NhaCungCaps, "MaNhaCungCap", "Std", matHang.MaNhaCungCap);
            ViewData["MaThuongHieu"] = new SelectList(_context.ThuongHieus, "MaThuongHieu", "Slug", matHang.MaThuongHieu);
            return View(matHang);
        }

        // GET: Admin/AdminMatHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matHang = await _context.MatHangs.FindAsync(id);
            if (matHang == null)
            {
                return NotFound();
            }
            ViewData["MaDanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "Slug", matHang.MaDanhMuc);
            ViewData["MaKichCo"] = new SelectList(_context.KichCos, "MaKichCo", "MaKichCo", matHang.MaKichCo);
            ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac", matHang.MaMauSac);
            ViewData["MaNhaCungCap"] = new SelectList(_context.NhaCungCaps, "MaNhaCungCap", "Std", matHang.MaNhaCungCap);
            ViewData["MaThuongHieu"] = new SelectList(_context.ThuongHieus, "MaThuongHieu", "Slug", matHang.MaThuongHieu);
            return View(matHang);
        }

        // POST: Admin/AdminMatHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaMatHang,TenMatHang,GiaBan,DangDuocBan,SoSao,SoLuong,SoLuongDaBan,MoTa,DangDuocHienThi,MaNhaCungCap,MaThuongHieu,MaDanhMuc,MaKichCo,MaMauSac")] MatHang matHang)
        {
            if (id != matHang.MaMatHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatHangExists(matHang.MaMatHang))
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
            ViewData["MaDanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "Slug", matHang.MaDanhMuc);
            ViewData["MaKichCo"] = new SelectList(_context.KichCos, "MaKichCo", "MaKichCo", matHang.MaKichCo);
            ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac", matHang.MaMauSac);
            ViewData["MaNhaCungCap"] = new SelectList(_context.NhaCungCaps, "MaNhaCungCap", "Std", matHang.MaNhaCungCap);
            ViewData["MaThuongHieu"] = new SelectList(_context.ThuongHieus, "MaThuongHieu", "Slug", matHang.MaThuongHieu);
            return View(matHang);
        }

        // GET: Admin/AdminMatHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matHang = await _context.MatHangs
                .Include(m => m.MaDanhMucNavigation)
                .Include(m => m.MaKichCoNavigation)
                .Include(m => m.MaMauSacNavigation)
                .Include(m => m.MaNhaCungCapNavigation)
                .Include(m => m.MaThuongHieuNavigation)
                .FirstOrDefaultAsync(m => m.MaMatHang == id);
            if (matHang == null)
            {
                return NotFound();
            }

            return View(matHang);
        }

        // POST: Admin/AdminMatHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var matHang = await _context.MatHangs.FindAsync(id);
            _context.MatHangs.Remove(matHang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatHangExists(int id)
        {
            return _context.MatHangs.Any(e => e.MaMatHang == id);
        }
    }
}
