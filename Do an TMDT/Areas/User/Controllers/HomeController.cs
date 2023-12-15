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
using MailKit.Net.Smtp;
using System.Threading.Tasks;

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

        public IActionResult Brand(string? key,int? MaDM,int? MaTH,int? MaMau)
        {
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

            HttpContext.Session.SetInt32("IDSP", MaDM == null ? default(int) : MaDM.Value);
            HttpContext.Session.SetInt32("MaDM", MaDM == null ? default(int) : MaDM.Value);
            HttpContext.Session.SetInt32("MaTH", MaTH == null ? default(int) : MaTH.Value);
            HttpContext.Session.SetInt32("MaMau", MaMau == null ? default(int) : MaMau.Value);
            return View(model);

        }

    }

}
