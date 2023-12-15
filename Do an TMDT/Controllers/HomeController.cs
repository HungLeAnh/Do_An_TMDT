using Do_an_CCNPMM.Models;
using Do_an_CCNPMM.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Do_an_CCNPMM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WEBBANGIAYContext _context;

        public HomeController(ILogger<HomeController> logger, WEBBANGIAYContext context)
        {
            _logger = logger;
            _context = context;

        }
        public IActionResult ThuongHieu(string? key,int? MaDM, int? MaTH, int? MaMau)
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
            if (MaTH != null)
            {
                listSP = listSP.Where(x => x.MaThuongHieu == MaTH).ToList();
            }
            if (MaMau != null)
            {
                listSP = listSP.Where(x => x.MaMauSac == MaMau).ToList();
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
            
            HttpContext.Session.SetInt32("IDSP", MaDM==null?0:0);
            return View(model);

        }
        public IActionResult Loadsanpham(int? page , int MaLoai)
        {
          
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 8;
            var listcate = _context.ThuongHieus.AsNoTracking().ToList();
            ViewBag.listcate = listcate;
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
                foreach (var item_anh in listanh) {
                    mh.MatHangAnhs = listanh.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                }
                foreach (var item_TH in listTH)
                {
                    mh.thuonghieu = listTH.Where(x => x.MaThuongHieu == item.MaThuongHieu).ToList();
                }
                listSPW.Add(mh);
                model.MatHangs = listSPW;


            }
 
           
            PagedList<MatHangHome> models = new PagedList<MatHangHome>(model.MatHangs.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.list = models;
            return View(model);
        }
            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();//new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        }
    }
}
