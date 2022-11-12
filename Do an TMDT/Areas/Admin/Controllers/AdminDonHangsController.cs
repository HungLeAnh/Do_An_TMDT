using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList.Core;

namespace Do_an_TMDT.Areas.Admin.Controllers
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
        public async Task<IActionResult> Index(int? page,string tinhTrang="")
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

            var lsTinhTrang = _context.DonHangs.Distinct().Select(m=>new SelectListItem() { Text = m.TinhTrang, Value=m.TinhTrang, Selected = (m.TinhTrang == tinhTrang?true:false) });
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

            return View(donHang);
        }

        // GET: Admin/AdminDonHangs/Create
        public IActionResult Create()
        {
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "Email");
            ViewData["MaNguoiGiaoHang"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "Email");
            return View();
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
