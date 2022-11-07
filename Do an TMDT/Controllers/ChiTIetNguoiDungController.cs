using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;
using Do_an_TMDT.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Do_an_TMDT.Controllers
{
    public class ChiTIetNguoiDungController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public ChiTIetNguoiDungController(WEBBANGIAYContext context)
        {
            _context = context;
        }

        // GET: NguoiDungs2
        public async Task<IActionResult> ChiTietNG()
        {
            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            if (taikhoanID != null)
            {
                var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
                ViewBag.khachhang = khachhang[0];
                return View();
            }
            return View();
            

        }
        public async Task<IActionResult> DonHang()
        {
            
            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            if (taikhoanID != null)
            {
                var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
                ViewBag.khachhang = khachhang[0];
                var donhang = _context.DonHangs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
                ViewBag.donhang = donhang;
                return View();
            }
            return View();


        }
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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {
            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
            nguoiDung.MaLoaiNguoiDung = khachhang[0].MaLoaiNguoiDung;
            nguoiDung.MaNguoiDung = khachhang[0].MaNguoiDung;
            nguoiDung.MatKhauHash = khachhang[0].MatKhauHash;
            nguoiDung.Salt = khachhang[0].Salt;
            if (!ModelState.IsValid)
            {
                if (id != nguoiDung.MaNguoiDung)
                {
                    return NotFound();
                }
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
                return RedirectToAction(nameof(ChiTietNG));
            }
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
            return View();
        }
        public async Task<IActionResult> EditMK(int? id)
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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMK(int id,NguoiDung nguoiDung)
        {
            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
            nguoiDung.MaLoaiNguoiDung = khachhang[0].MaLoaiNguoiDung;
            nguoiDung.MaNguoiDung = khachhang[0].MaNguoiDung;
            if (!ModelState.IsValid)
            {
                if (id != nguoiDung.MaNguoiDung)
                {
                    return NotFound();
                }
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
                return RedirectToAction(nameof(ChiTietNG));
            }
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
            return View();
        }
        public async Task<IActionResult> SoDiaChi()
        {
           
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoDiaChi(int id, [Bind("DiaChi")] HomeVM Home)
        {
            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
            NguoiDungDiaChi nd = new NguoiDungDiaChi
            {
                MaNguoiDung = Convert.ToInt32(taikhoanID),
                DiaChi = Home.DiaChi,

            };
           
                 _context.Add(nd);
                await _context.SaveChangesAsync();
                var listdiachi = _context.NguoiDungDiaChis.Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
                ViewBag.diachi = listdiachi;
                return RedirectToAction(nameof(SoDiaChi));
            
            
        }
        private bool NguoiDungExists(int id)
        {
            return _context.NguoiDungs.Any(e => e.MaNguoiDung == id);
        }


    }
}
