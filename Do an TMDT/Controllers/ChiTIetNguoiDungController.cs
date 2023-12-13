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
using Do_an_TMDT.Extension;
using PagedList.Core;

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
        public IActionResult ChiTietNG()
        {
            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            if (taikhoanID != null)
            {
                var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).FirstOrDefault();
                ViewBag.khachhang = khachhang;
                return View();
            }
            return View();
            

        }
        public async Task<IActionResult> DonHang(int? page)
        {

            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 3;
            var lsdonhang = await _context.DonHangs.Where(m => m.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToListAsync();
            PagedList<DonHang> model = new PagedList<DonHang>(lsdonhang.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            if (taikhoanID != null)
            {                
                var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).FirstOrDefault();
                ViewBag.khachhang = khachhang;
                return View(model);
            }
            return View();


        }
        public async Task<IActionResult> ChiTietDH(int MaDH)
        {

            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            HttpContext.Session.SetInt32("MaDH",MaDH);
            if (taikhoanID != null)
            {
                var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).FirstOrDefault();
                ViewBag.khachhang = khachhang;
                var donhang2 = _context.DonHangs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
                ViewBag.donhang = donhang2;
             
            }
            var donhang = _context.DonHangs.Where(x => x.MaDonHang == MaDH).FirstOrDefault();
            ViewBag.tinhtrang = donhang.TinhTrang;
            var listsp_donhang = _context.ChiTietDonHangs.Where(x => x.MaDonHang == MaDH).ToList();
            List<CTDH> ct = new List<CTDH>();
            foreach (var item in listsp_donhang)
            {
                CTDH ct2 = new CTDH();
                ct2.CT_DH = item;
                ct2.SanPham = await _context.MatHangs.FindAsync(item.MaMatHang);
                ct.Add(ct2);
            }
            ViewBag.chitietdonhang = ct;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChiTietDH([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {

            int MaDH= (int)HttpContext.Session.GetInt32("MaDH");
            var taikhoanID = HttpContext.Session.GetInt32("Ten");
        
            if (taikhoanID != null)
            {
                var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
                ViewBag.khachhang = khachhang[0];
                var donhang2 = _context.DonHangs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
                ViewBag.donhang = donhang2;

            }
            var donhang = _context.DonHangs.Where(x => x.MaDonHang == MaDH).FirstOrDefault();
                 ViewBag.tinhtrang = donhang.TinhTrang;
            if (donhang.TinhTrang == "Chưa xác nhận")
            {
               
                donhang.TinhTrang = "Đã xác nhận";
                _context.Update(donhang);
                await _context.SaveChangesAsync();
            }
            if (donhang.TinhTrang == "Đã giao")
            {

                donhang.TinhTrang = "Đơn hàng thành công";
                donhang.DaThanhToan = true;
                _context.Update(donhang);
                await _context.SaveChangesAsync();
            }
            ViewBag.sus = "Đã xác nhận thành công";
           
            var listsp_donhang = _context.ChiTietDonHangs.Where(x => x.MaDonHang == MaDH).ToList();
            List<CTDH> ct = new List<CTDH>();
            foreach(var item in listsp_donhang)
            {
                CTDH ct2 = new CTDH();
                ct2.CT_DH = item;
                ct2.SanPham = await _context.MatHangs.FindAsync(item.MaMatHang);
                ct.Add(ct2);
            }
            ViewBag.chitietdonhang = ct;
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
            nguoiDung.Email = khachhang[0].Email;
            nguoiDung.TenDangNhap = khachhang[0].TenDangNhap;
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
        public IActionResult EditMK(int id, [Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {


            
            
                var taikhoanID = HttpContext.Session.GetInt32("Ten");
                var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
                nguoiDung.MaLoaiNguoiDung = khachhang[0].MaLoaiNguoiDung;
                nguoiDung.MaNguoiDung = khachhang[0].MaNguoiDung;
                nguoiDung.TenNguoiDung = khachhang[0].TenNguoiDung;
                nguoiDung.AnhDaiDien = khachhang[0].AnhDaiDien;
                nguoiDung.TenDangNhap = khachhang[0].TenDangNhap;
                nguoiDung.Email = khachhang[0].Email;
                nguoiDung.Sdt = khachhang[0].Sdt;
                nguoiDung.ViDienTu = khachhang[0].ViDienTu;
                nguoiDung.Salt = khachhang[0].Salt;
                nguoiDung.MatKhauHash=(nguoiDung.MatKhauHash+ khachhang[0].Salt.Trim()).ToMD5();
            if (nguoiDung.MatKhauHash != khachhang[0].MatKhauHash)
            {

                return View();

            }
            else
            {
                HttpContext.Session.SetInt32("Ten", nguoiDung.MaNguoiDung);
                return RedirectToAction("MKMoi", new { id = khachhang[0].MaNguoiDung });
            }
            //    if (!ModelState.IsValid)
            //{
            //    if (id != nguoiDung.MaNguoiDung)
            //    {
            //        return NotFound();
            //    }
            //    try
            //    {
            //        _context.Update(nguoiDung);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!NguoiDungExists(nguoiDung.MaNguoiDung))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(ChiTietNG));
            //}
            //ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
            //
            
            ViewBag.mess = "sai mật khẩu";
            return View();


        }

        
        public IActionResult SoDiaChi()
        {
            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            var listdiachi = _context.NguoiDungDiaChis.Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
            ViewBag.diachi = listdiachi;
          
                var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).FirstOrDefault();
                ViewBag.khachhang = khachhang;
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

        public async Task<IActionResult> MKMoi(int? id)
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
        public async Task<IActionResult> MKMoi(int id, [Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {




            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
            nguoiDung.MaLoaiNguoiDung = khachhang[0].MaLoaiNguoiDung;
            nguoiDung.MaNguoiDung = khachhang[0].MaNguoiDung;
            nguoiDung.TenNguoiDung = khachhang[0].TenNguoiDung;
            nguoiDung.AnhDaiDien = khachhang[0].AnhDaiDien;
            nguoiDung.TenDangNhap = khachhang[0].TenDangNhap;
            nguoiDung.Email = khachhang[0].Email;
            nguoiDung.Sdt = khachhang[0].Sdt;
            nguoiDung.ViDienTu = khachhang[0].ViDienTu;
            nguoiDung.Salt = khachhang[0].Salt;
            nguoiDung.MatKhauHash = (nguoiDung.MatKhauHash + khachhang[0].Salt.Trim()).ToMD5();


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
            //

            return View();


        }
       
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuongHieu = _context.DonHangs
                .FirstOrDefault(m => m.MaDonHang == id);
            HttpContext.Session.SetInt32("MaDH", id);
            if (thuongHieu == null)
            {
                return NotFound();
            }
            ViewBag.xoa = thuongHieu;

            return View();
        }

        // POST: Admin/AdminThuongHieus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            int MaDH = (int)HttpContext.Session.GetInt32("MaDH");
            var thuongHieu = await _context.DonHangs.FindAsync(MaDH);
            var ChiTietDH =  _context.ChiTietDonHangs.Where(x=>x.MaDonHang==MaDH).ToList();
           

            if (thuongHieu.TinhTrang=="Chưa xác nhận")
            {
                foreach (var item in ChiTietDH)
                {
                    _context.Remove(item);
                    await _context.SaveChangesAsync();
                }
                
                _context.Remove(thuongHieu);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(DonHang));
            }
            ViewBag.xoa = thuongHieu;
            ViewBag.mess = "Đơn hàng đã đóng gói hoặc đã xác nhận không thể xóa";
            return View();



        }


    }
}
