using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Do_an_TMDT.ViewModels;
using Do_an_TMDT.Helpper;
using Do_an_TMDT.Extension;

namespace Do_an_TMDT.Controllers
{
    public class NguoiDungsController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public NguoiDungsController(WEBBANGIAYContext context)
        {
            _context = context;
        }

        public IActionResult Loadsanpham()
        {
            HomeVM model = new HomeVM();
            var listSP = _context.MatHangs.AsNoTracking()
                .Where(x => x.DangDuocBan == true)
                .ToList();
            List<MatHangHome> listSPW = new List<MatHangHome>();
            var listanh = _context.MatHangAnhs
                .AsNoTracking()
                .ToList();
            var listTH = _context.ThuongHieus
                .AsNoTracking()
                .ToList();
            foreach (var item_TH in listTH)
            {
                model.TH = listTH.Where(x => x.MaThuongHieu != null).ToList();
            }
            foreach (var item in listSP)
            {

                MatHangHome mh = new MatHangHome();
                mh.listSPs = item;
                foreach (var item_anh in listanh)
                {
                    mh.MatHangAnhs = listanh.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                }
                foreach (var item_TH in listTH)
                {
                    mh.thuonghieu = listTH.Where(x => x.MaThuongHieu == item.MaThuongHieu).ToList();
                }
                listSPW.Add(mh);
                model.MatHangs = listSPW;
                ViewBag.mathang = listSPW;
                ViewBag.Id = HttpContext.Session.GetInt32("Ten");


            }
            return View();
        }


        public IActionResult dangky()
        {
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> dangky([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                bool x = true;
                string salt = Utilities.GetRandomKey();
                var listLoaiNguoidung = _context.LoaiNguoiDungs.AsNoTracking().ToList();
                NguoiDung khachhang = new NguoiDung
                {
                    MaLoaiNguoiDung = nguoiDung.MaLoaiNguoiDung,
                    TenDangNhap=nguoiDung.TenDangNhap,
                    TenNguoiDung = nguoiDung.TenNguoiDung,
                    Sdt = nguoiDung.Sdt.Trim().ToLower(),
                    Email = nguoiDung.Email.Trim().ToLower(),
                    MatKhauHash = (nguoiDung.MatKhauHash + salt.Trim()).ToMD5(),
                    Salt = salt
                };
                var list = _context.NguoiDungs.AsNoTracking().ToList();
                foreach (var item in list)
                {
                    if (khachhang.Email == item.Email)
                    {
                        ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
                        ViewBag.mess = "Email đã tồn tại";
                        x = false;
                        return View();
                    }
                    if (khachhang.Sdt == item.Sdt)
                    {
                        ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
                        ViewBag.mess = " SDT đã tồn tại";
                        x = false;
                        return View();
                    }
                    if (khachhang.TenDangNhap == item.TenDangNhap)
                    {
                        ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
                        ViewBag.mess = " Tên Đăng Nhập đã tồn tại";
                        x = false;
                        return View();
                    }
                }
                if (x == true)
                {
                    if (khachhang.Sdt.Length == 10)
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(dangnhap));
                    }
                    else
                    {
                        ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
                        ViewBag.mess = " SDT không hợp lệ";
                        x = false;
                        return View();
                    }
            }                
            }
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
            return View();
        }

        public IActionResult dangnhap()
        {
          
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> dangnhap([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {
            int i = 0;
            var listLoaiNguoidung = _context.LoaiNguoiDungs.AsNoTracking().ToList();
            var list = _context.NguoiDungs.AsNoTracking().ToList();
            string[] MaLoaiNguoiDung = new string[listLoaiNguoidung.Count()];
            foreach (var item in listLoaiNguoidung)
            {
                MaLoaiNguoiDung[i] = item.MaLoaiNguoiDung;
                i++;
            }
            foreach (var item in list)
            {
                if (nguoiDung.Email == item.Email || nguoiDung.Email==item.TenDangNhap)
                {
                    string pass = (nguoiDung.MatKhauHash + item.Salt.Trim()).ToMD5();
                    if (pass == item.MatKhauHash)
                    {
                        String x = item.MaLoaiNguoiDung;
                        if (x == MaLoaiNguoiDung[0])
                        {
                            int id = item.MaNguoiDung;

                            HttpContext.Session.SetInt32("Ten", item.MaNguoiDung);
                            return RedirectToAction("Loadsanpham");
                        }
                        else if (x == MaLoaiNguoiDung[1])
                        {
                            int id = item.MaNguoiDung;

                            HttpContext.Session.SetInt32("Ten", item.MaNguoiDung);
                            return RedirectToAction("", "Home", new { area = "Admin" });
                        }

                    }
                    ViewBag.mess = " Mật khẩu không đúng";

                }
                ViewBag.mess = " Tên đăng nhập không đúng";


            }

            return View();
        }



    }
}
