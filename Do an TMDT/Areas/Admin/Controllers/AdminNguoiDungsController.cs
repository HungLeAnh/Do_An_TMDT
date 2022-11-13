using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;
using Do_an_TMDT.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList.Core;
using Do_an_TMDT.Extension;
using Do_an_TMDT.Helpper;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;

namespace Do_an_TMDT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminNguoiDungsController : Controller
    {
        private readonly WEBBANGIAYContext _context;
        public INotyfService _notyfService { get; }

        public AdminNguoiDungsController(WEBBANGIAYContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;

        }

        // GET: Admin/AdminNguoiDungs
        public IActionResult Index(int? page,string maLoaiNguoiDung = "")
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsCustomers = new List<NguoiDung>();
            maLoaiNguoiDung = maLoaiNguoiDung.Trim();
            if (maLoaiNguoiDung != "")
            {
                lsCustomers = _context.NguoiDungs.Where(x => x.MaLoaiNguoiDungNavigation.MaLoaiNguoiDung == maLoaiNguoiDung)
                    .AsNoTracking()
                    .OrderByDescending(x => x.MaNguoiDung)
                    .Include(n => n.MaLoaiNguoiDungNavigation).ToList();
            }
            else
            {
                lsCustomers = _context.NguoiDungs
                    .AsNoTracking()
                    .OrderByDescending(x => x.MaNguoiDung)
                    .Include(n => n.MaLoaiNguoiDungNavigation).ToList();
            }
            PagedList<NguoiDung> models = new PagedList<NguoiDung>(lsCustomers.AsQueryable(), pageNumber, pageSize);
            
            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentLoaiNguoiDung = maLoaiNguoiDung;

            ViewData["LoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "TenLoaiNguoiDung",maLoaiNguoiDung);
            return View(models);
        }
        public IActionResult Filtter(string loaiNguoiDung ="")
        {
            var url = $"/Admin/AdminNguoiDungs?MaLoaiNguoiDung={loaiNguoiDung.Trim()}";
            if (loaiNguoiDung == "")
            {
                url = $"/Admin/AdminNguoiDungs";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        // GET: Admin/AdminNguoiDungs/Details/5
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

        // GET: Admin/AdminNguoiDungs/Create
        public IActionResult Create()
        {
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung");
            return View();
        }

        // POST: Admin/AdminNguoiDungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
            if (ModelState.IsValid)
            {
                bool x = true;
                string salt = Utilities.GetRandomKey();
                var listLoaiNguoidung = _context.LoaiNguoiDungs.AsNoTracking().ToList();
                NguoiDung khachhang = new NguoiDung
                {
                    MaLoaiNguoiDung = nguoiDung.MaLoaiNguoiDung,
                    TenDangNhap = nguoiDung.TenDangNhap,
                    TenNguoiDung = nguoiDung.TenNguoiDung,
                    Sdt = nguoiDung.Sdt.Trim(),
                    Email = nguoiDung.Email.Trim(),
                    MatKhauHash = (nguoiDung.MatKhauHash + salt.Trim()).ToMD5(),
                    Salt = salt
                };
                var list = _context.NguoiDungs.AsNoTracking().ToList();
                foreach (var item in list)
                {
                    if (khachhang.Email == item.Email)
                    {
                        ViewBag.mess = "Email đã tồn tại";
                        x = false;
                        break;
                    }
                    if (khachhang.Sdt == item.Sdt)
                    {
                        ViewBag.mess = " SDT đã tồn tại";
                        x = false;
                        break;
                    }
                    if (khachhang.TenDangNhap == item.TenDangNhap)
                    {
                        ViewBag.mess = " Tên Đăng Nhập đã tồn tại";
                        x = false;
                        break;

                    }
                }
                if (x == true)
                {
                    if (khachhang.Sdt.Length == 10)
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
                        ViewBag.mess = " SDT không hợp lệ";
                        x = false;
                        return View(nguoiDung);
                    }
                }
            }

            return View(nguoiDung);
        }

        // GET: Admin/AdminNguoiDungs/Edit/5
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

        // POST: Admin/AdminNguoiDungs/Edit/5
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

        // GET: Admin/AdminNguoiDungs/Delete/5
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

        // POST: Admin/AdminNguoiDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donhang = _context.DonHangs.Where(x => x.MaNguoiDung == id).ToList();
            foreach (var item in donhang)
            {
                _context.DonHangs.Remove(item);
            }
            var nguoi_DC = _context.NguoiDungDiaChis.Where(x => x.MaNguoiDung == id).ToList();
            foreach (var item in nguoi_DC)
            {
                _context.NguoiDungDiaChis.Remove(item);
            }
            var giohang = _context.GioHangs.Where(x => x.MaNguoiDung == id).ToList();
            foreach (var item in giohang)
            {
                _context.GioHangs.Remove(item);
            }

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
