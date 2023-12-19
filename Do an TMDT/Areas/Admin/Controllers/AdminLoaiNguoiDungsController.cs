using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_CCNPMM.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using PagedList.Core;
using Microsoft.AspNetCore.Authorization;



namespace Do_an_CCNPMM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminLogin")]

    public class AdminLoaiNguoiDungsController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public INotyfService _notyfService { get; }

        public AdminLoaiNguoiDungsController(WEBBANGIAYContext context,INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminLoaiNguoiDungs
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsLoaiNguoiDung = await _context.LoaiNguoiDungs.ToListAsync();
            PagedList<LoaiNguoiDung> model = new PagedList<LoaiNguoiDung>(lsLoaiNguoiDung.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(model);
        }

        // GET: Admin/AdminLoaiNguoiDungs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiNguoiDung = await _context.LoaiNguoiDungs
                .FirstOrDefaultAsync(m => m.MaLoaiNguoiDung == id);
            if (loaiNguoiDung == null)
            {
                return NotFound();
            }

            return View(loaiNguoiDung);
        }

        // GET: Admin/AdminLoaiNguoiDungs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminLoaiNguoiDungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoaiNguoiDung,TenLoaiNguoiDung")] LoaiNguoiDung loaiNguoiDung)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiNguoiDung);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(loaiNguoiDung);

        }

        // GET: Admin/AdminLoaiNguoiDungs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiNguoiDung = await _context.LoaiNguoiDungs.FindAsync(id);
            if (loaiNguoiDung == null)
            {
                return NotFound();
            }
            return View(loaiNguoiDung);
        }

        // POST: Admin/AdminLoaiNguoiDungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaLoaiNguoiDung,TenLoaiNguoiDung")] LoaiNguoiDung loaiNguoiDung)
        {
            if (id != loaiNguoiDung.MaLoaiNguoiDung)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiNguoiDung);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiNguoiDungExists(loaiNguoiDung.MaLoaiNguoiDung))
                    {
                        _notyfService.Error("Có lỗi xảy ra");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loaiNguoiDung);
        }

/*        // GET: Admin/AdminLoaiNguoiDungs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiNguoiDung = await _context.LoaiNguoiDungs
                .FirstOrDefaultAsync(m => m.MaLoaiNguoiDung == id);
            if (loaiNguoiDung == null)
            {
                return NotFound();
            }

            return View(loaiNguoiDung);
        }*/

        // POST: Admin/AdminLoaiNguoiDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var loaiNguoiDung = await _context.LoaiNguoiDungs.FindAsync(id);
            _context.LoaiNguoiDungs.Remove(loaiNguoiDung);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiNguoiDungExists(string id)
        {
            return _context.LoaiNguoiDungs.Any(e => e.MaLoaiNguoiDung == id);
        }
    }
}
