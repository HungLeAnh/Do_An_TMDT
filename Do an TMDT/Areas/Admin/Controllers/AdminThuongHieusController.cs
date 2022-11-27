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
using AspNetCoreHero.ToastNotification.Notyf;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Do_an_TMDT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminThuongHieusController : Controller
    {
        private readonly WEBBANGIAYContext _context;
        public INotyfService _notyfService { get; }

        public AdminThuongHieusController(WEBBANGIAYContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;

        }

        // GET: Admin/AdminThuongHieus
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsThuongHieu = await _context.ThuongHieus.ToListAsync();
            PagedList<ThuongHieu> model = new PagedList<ThuongHieu>(lsThuongHieu.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(model);
        }

        // GET: Admin/AdminThuongHieus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus
                .FirstOrDefaultAsync(m => m.MaThuongHieu == id);
            if (thuongHieu == null)
            {
                return NotFound();
            }

            return View(thuongHieu);
        }

        // GET: Admin/AdminThuongHieus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminThuongHieus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaThuongHieu,TenThuongHieu,Slug")] ThuongHieu thuongHieu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thuongHieu);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(thuongHieu);
        }

        // GET: Admin/AdminThuongHieus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus.FindAsync(id);
            if (thuongHieu == null)
            {
                return NotFound();
            }
            return View(thuongHieu);
        }

        // POST: Admin/AdminThuongHieus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaThuongHieu,TenThuongHieu,Slug")] ThuongHieu thuongHieu)
        {
            if (id != thuongHieu.MaThuongHieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thuongHieu);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThuongHieuExists(thuongHieu.MaThuongHieu))
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
            return View(thuongHieu);
        }

        // GET: Admin/AdminThuongHieus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = await _context.ThuongHieus
                .FirstOrDefaultAsync(m => m.MaThuongHieu == id);
            if (thuongHieu == null)
            {
                return NotFound();
            }

            return View(thuongHieu);
        }

        // POST: Admin/AdminThuongHieus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thuongHieu = await _context.ThuongHieus.FindAsync(id);
            _context.ThuongHieus.Remove(thuongHieu);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool ThuongHieuExists(int id)
        {
            return _context.ThuongHieus.Any(e => e.MaThuongHieu == id);
        }
    }
}
