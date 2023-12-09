using AspNetCoreHero.ToastNotification.Abstractions;
using Do_an_TMDT.Extension;
using Do_an_TMDT.Helpper;
using Do_an_TMDT.Models;
using Do_an_TMDT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace Do_an_TMDT.Areas.User.Controllers
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

        public IActionResult ThuongHieu(int? MaTH)
        {
            HomeVM model = new HomeVM();
            var listcate = _context.ThuongHieus.AsNoTracking().ToList();
            ViewBag.listcate = listcate;

            var danhmuc = _context.DanhMucs.AsNoTracking().ToList();
            ViewBag.danhmuc = danhmuc;

            var mausac = _context.MauSacs.AsNoTracking().ToList();
            ViewBag.mausac = mausac;

            var listSP = new List<MatHang>();
            if (MaTH != null)
            {
                listSP = _context.MatHangs.AsNoTracking()
                    .Where(x => x.DangDuocBan == true)
                    .Where(x => x.MaThuongHieu == MaTH)
                    .ToList();

            }
            else
            {
                listSP = _context.MatHangs.AsNoTracking()
                      .Where(x => x.DangDuocBan == true)
                      .ToList();
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

            HttpContext.Session.SetInt32("IDSP", MaTH == null ? 0 : 0);
            return View(model);

        }


        public IActionResult Timkiem(NguoiDung timkiem)
        {
            var listcate = _context.ThuongHieus.AsNoTracking().ToList();
            ViewBag.listcate = listcate;
            HomeVM model = new HomeVM();
            List<MatHang> listSP = null;
            if (!String.IsNullOrEmpty(timkiem.TenNguoiDung))
            {
                timkiem.TenNguoiDung = timkiem.TenNguoiDung.ToLower();
                listSP = _context.MatHangs.Where(b => b.TenMatHang.ToLower().Contains(timkiem.TenNguoiDung)).ToList();
            }
            else
            {
                return View();
            }
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

            }
            ViewBag.Id = HttpContext.Session.GetInt32("Ten");

            return View();
        }
    }

}
