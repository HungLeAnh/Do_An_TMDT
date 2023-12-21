using AspNetCoreHero.ToastNotification.Abstractions;
using Do_an_CCNPMM.Extension;
using Do_an_CCNPMM.Helpper;
using Do_an_CCNPMM.Models;
using Do_an_CCNPMM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using PagedList.Core;

namespace Do_an_CCNPMM.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(AuthenticationSchemes = "UserLogin")]
    public class HomeController : Controller
    {
        private readonly WEBBANGIAYContext _context;
        public INotyfService _notyfService { get; }
        public HomeController(WEBBANGIAYContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index()
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
            var listcate = _context.ThuongHieus.AsNoTracking().ToList();
            ViewBag.listcate = listcate;
            ViewBag.Id = HttpContext.Session.GetInt32("Ten");

            HttpContext.Session.SetInt32("IDSP", 0);
            return View();
        }

        public IActionResult Brand(int? page, string? key,int? MaDM,int? MaTH,int? MaMau)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 6;

            HomeVM model = new HomeVM();
            var listcate = _context.ThuongHieus.AsNoTracking().ToList();
            ViewBag.listcate = listcate;

            var danhmuc = _context.DanhMucs.AsNoTracking().ToList();
            ViewBag.danhmuc = danhmuc;

            var mausac = _context.MauSacs.AsNoTracking().ToList();
            ViewBag.mausac = mausac;

            var listSP = new List<MatHang>();
            listSP = _context.MatHangs.AsNoTracking()
                      .Where(x => x.DangDuocBan == true)
                      .ToList();
            if (!String.IsNullOrEmpty(key))
            {
                key = key.Trim().ToLower();
                listSP = _context.MatHangs.Where(b => b.TenMatHang.ToLower().Contains(key)).ToList();
            }
            if (MaDM != null)
            {
                listSP = listSP.Where(x => x.MaDanhMuc == MaDM).ToList();

            }
            if(MaTH != null)
            {
                listSP = listSP.Where(x => x.MaThuongHieu == MaTH).ToList();
            }
            if(MaMau != null)
            {
                listSP = listSP.Where(x=> x.MaMauSac == MaMau).ToList();
            }
            List<MatHangHome> listSPW = new List<MatHangHome>();
            var listanh = _context.MatHangAnhs
                .AsNoTracking()
                .ToList();
            var listTH = _context.ThuongHieus
                .AsNoTracking()
                .ToList();

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

            }
            if(model.MatHangs != null)
            {
                PagedList<MatHangHome> mahangpaging = new PagedList<MatHangHome>(model.MatHangs.AsQueryable(), pageNumber, pageSize);
                model.MatHangPaging = mahangpaging;

            }

            ViewBag.CurrentPage = pageNumber;

            HttpContext.Session.SetInt32("IDSP", MaDM == null ? default(int) : MaDM.Value);
            HttpContext.Session.SetInt32("MaDM", MaDM == null ? default(int) : MaDM.Value);
            HttpContext.Session.SetInt32("MaTH", MaTH == null ? default(int) : MaTH.Value);
            HttpContext.Session.SetInt32("MaMau", MaMau == null ? default(int) : MaMau.Value);
            return View(model);
        }

        public IActionResult AddCart(int? id)
        {
            int sl = 1;
            string message;
            CartVM cart = new CartVM();
            List<itemcart> itemcarts = new List<itemcart>();
            var listSP = _context.MatHangs
                .AsNoTracking().Where(x => x.DangDuocBan == true)
                .ToList();
            var listanh = _context.MatHangAnhs
                .AsNoTracking()
                .ToList();
            var listTH = _context.ThuongHieus
                .AsNoTracking()
                .ToList();
            int idgh = (int)HttpContext.Session.GetInt32("GH");
            var listGH = _context.ChiTietGioHangs.Where(x => x.MaGioHang == idgh)
               .AsNoTracking()
               .ToList();
            foreach (var item in listGH)
            {
                itemcart it = new itemcart();
                it.CT_GH = item;
                var SP = listSP.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                it.SanPham = SP[0];
                foreach (var item_anh in listanh)
                {
                    it.MatHangAnhs = listanh.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                }
                itemcarts.Add(it);
            }
            cart.item = itemcarts;
            int MaSp;
            if (HttpContext.Session.GetInt32("IDSP") != null)
            {
                MaSp = (int)HttpContext.Session.GetInt32("IDSP");
            }
            else
            {
                MaSp = id.GetValueOrDefault();
            }

            ViewBag.Id = HttpContext.Session.GetInt32("Ten");
            if (MaSp != 0) { id = MaSp; }
            var list = listGH.Where(x => x.MaMatHang == id).ToList();
            //kiểm tra mặt hàng và mã gio hàng có chưa
            var listCheck = listGH.Where(x => x.MaGioHang == idgh).ToList();
            var listCheck2 = listCheck.Where(x => x.MaMatHang == id).ToList();
            var list2 = listGH.Where(x => x.MaMatHang == id).ToList();
            var mathang = listSP.Where(x => x.MaMatHang == id).ToList();

            var sanpham = _context.MatHangs.Where(x => x.MaMatHang == id).FirstOrDefault();

            if (listCheck2.Count == 0)
            {
                if (sanpham.SoLuong <  sl)
                {
                    message = "Không đủ số lượng sản phẩm trong kho";
                    return Json(new { Message = message });

                }
                ChiTietGioHang tc = new ChiTietGioHang
                {
                    MaGioHang = idgh,
                    MaMatHang = id.GetValueOrDefault(),
                    SoLuong = sl,
                    Gia = (int)mathang[0].GiaBan,
                };

                _context.Add(tc);
                _context.SaveChanges();
                CartVM cartNew = new CartVM();
                List<itemcart> itemcartsNew = new List<itemcart>();
                var listGHNew = _context.ChiTietGioHangs.Where(x => x.MaGioHang == idgh)
               .AsNoTracking()
               .ToList();
                long thanhtien = 0;
                foreach (var item in listGHNew)
                {
                    itemcart it = new itemcart();
                    it.CT_GH = item;
                    var SP = listSP.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                    it.SanPham = SP[0];
                    foreach (var item_anh in listanh)
                    {
                        it.MatHangAnhs = listanh.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                    }
                    itemcartsNew.Add(it);
                    it.tong = (int)(SP[0].GiaBan * item.SoLuong);
                    thanhtien += it.tong;
                }
                cartNew.item = itemcartsNew;
                ViewBag.thanhtien = thanhtien;
                ViewBag.giohang = cartNew;
                HttpContext.Session.SetString("thanhtien", thanhtien.ToString());
                HttpContext.Session.SetInt32("sl", 0);
                HttpContext.Session.SetInt32("IDSP", 0);
                message = "Thêm vào giỏ hàng thành công";
                return Json(new { Message = message}); 
            }
            else
            {
                if (sanpham.SoLuong < list[0].SoLuong + sl)
                {
                    message = "Không đủ số lượng sản phẩm trong kho";
                    return Json(new { Message = message });
                }
                ChiTietGioHang tc = new ChiTietGioHang
                {
                    MaGioHang = idgh,
                    MaMatHang = id.GetValueOrDefault(),
                    SoLuong = list[0].SoLuong + sl,
                    Gia = (int)mathang[0].GiaBan
                };
                ViewBag.giohang = listGH;
                _context.Update(tc);
                _context.SaveChanges();

                CartVM cartNew = new CartVM();
                List<itemcart> itemcartsNew = new List<itemcart>();
                var listGHNew = _context.ChiTietGioHangs.Where(x => x.MaGioHang == idgh)
               .AsNoTracking()
               .ToList();
                long thanhtien = 0;
                foreach (var item in listGHNew)
                {
                    itemcart it = new itemcart();
                    it.CT_GH = item;
                    var SP = listSP.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                    it.SanPham = SP[0];
                    foreach (var item_anh in listanh)
                    {
                        it.MatHangAnhs = listanh.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                    }
                    itemcartsNew.Add(it);
                    it.tong = (int)(SP[0].GiaBan * item.SoLuong);
                    thanhtien += it.tong;
                }
                cartNew.item = itemcartsNew;
                ViewBag.thanhtien = thanhtien;
                ViewBag.giohang = cartNew;
                HttpContext.Session.SetInt32("sl", 0);
                HttpContext.Session.SetString("thanhtien", thanhtien.ToString());
                HttpContext.Session.SetInt32("IDSP", 0);
                _notyfService.Success("Thêm vào giỏ hàng thành công");

                message = "Thêm vào giỏ hàng thành công";
                return Json(new { Message = message });
            }
        }
    }

}
