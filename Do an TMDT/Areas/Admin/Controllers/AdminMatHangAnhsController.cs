using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;

namespace Do_an_TMDT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMatHangAnhsController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public AdminMatHangAnhsController(WEBBANGIAYContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminMatHangAnhs
        public async Task<IActionResult> Index()
        {
            var wEBBANGIAYContext = _context.MatHangAnhs.Include(m => m.MaMatHangNavigation);
            return View(await wEBBANGIAYContext.ToListAsync());
        }

        // GET: Admin/AdminMatHangAnhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matHangAnh = await _context.MatHangAnhs
                .Include(m => m.MaMatHangNavigation)
                .FirstOrDefaultAsync(m => m.MaMatHang == id);
            if (matHangAnh == null)
            {
                return NotFound();
            }

            return View(matHangAnh);
        }

        // GET: Admin/AdminMatHangAnhs/Create
        public IActionResult Create()
        {
            ViewData["MaMatHang"] = new SelectList(_context.MatHangs, "MaMatHang", "MoTa");
            return View();
        }

        // POST: Admin/AdminMatHangAnhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaAnh,MaMatHang,Anh")] MatHangAnh matHangAnh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(matHangAnh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaMatHang"] = new SelectList(_context.MatHangs, "MaMatHang", "MoTa", matHangAnh.MaMatHang);
            return View(matHangAnh);
        }

        // GET: Admin/AdminMatHangAnhs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matHangAnh = await _context.MatHangAnhs.FindAsync(id);
            if (matHangAnh == null)
            {
                return NotFound();
            }
            ViewData["MaMatHang"] = new SelectList(_context.MatHangs, "MaMatHang", "MoTa", matHangAnh.MaMatHang);
            return View(matHangAnh);
        }

        // POST: Admin/AdminMatHangAnhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaAnh,MaMatHang,Anh")] MatHangAnh matHangAnh)
        {
            if (id != matHangAnh.MaMatHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matHangAnh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatHangAnhExists(matHangAnh.MaMatHang))
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
            ViewData["MaMatHang"] = new SelectList(_context.MatHangs, "MaMatHang", "MoTa", matHangAnh.MaMatHang);
            return View(matHangAnh);
        }

        // GET: Admin/AdminMatHangAnhs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matHangAnh = await _context.MatHangAnhs
                .Include(m => m.MaMatHangNavigation)
                .FirstOrDefaultAsync(m => m.MaMatHang == id);
            if (matHangAnh == null)
            {
                return NotFound();
            }

            return View(matHangAnh);
        }

        // POST: Admin/AdminMatHangAnhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var matHangAnh = await _context.MatHangAnhs.FindAsync(id);
            _context.MatHangAnhs.Remove(matHangAnh);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatHangAnhExists(int id)
        {
            return _context.MatHangAnhs.Any(e => e.MaMatHang == id);
        }
    }
}
