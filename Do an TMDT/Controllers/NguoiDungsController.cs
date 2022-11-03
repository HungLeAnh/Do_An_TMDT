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
                model.Ng = (int)HttpContext.Session.GetInt32("Ten");
            }
            return View(model);
        }


        public IActionResult dangky()
        {
            
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> dangky([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {
            bool x = true;
            var list = _context.NguoiDungs.AsNoTracking().ToList();
            foreach (var item in list)
            {
                if (nguoiDung.Email == item.Email)
                {
                    ViewBag.mess = "Email đã tồn tại";
                    x = false;
                    return View();
                }
            }
            if (x == true)
            {
                nguoiDung.MaLoaiNguoiDung = "KH";
                _context.Add(nguoiDung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(dangnhap));
            }
            if (ModelState.IsValid)
            {
            }
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
            if (ModelState.IsValid)
            {
               
                var list = _context.NguoiDungs.AsNoTracking().ToList();
                foreach (var item in list)
                {
                    if (nguoiDung.Email == item.Email && nguoiDung.MatKhauHash == item.MatKhauHash)
                    {
                        int x = int.Parse(item.MaLoaiNguoiDung);
                        if (x == 1)
                        {
                            int id = item.MaNguoiDung;

                            HttpContext.Session.SetInt32("Ten", item.MaNguoiDung);
                            return RedirectToAction("Loadsanpham");
                        }
                        else if (x == 2)
                        {
                            int id = item.MaNguoiDung;

                            HttpContext.Session.SetInt32("Ten", item.MaNguoiDung);
                            return RedirectToAction("","Home",new { area = "Admin" });
                        }
                    }
                }
            }
            return View();
        }



    }
}
