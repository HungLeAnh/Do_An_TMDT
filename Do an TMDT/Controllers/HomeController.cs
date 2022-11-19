using Do_an_TMDT.Models;
using Do_an_TMDT.ViewModels;
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

namespace Do_an_TMDT.Controllers
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
        public IActionResult Loadhet(int? page, int MaLoai)
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
            if (MaLoai == null)
            {
                MaLoai = listcate[0].MaThuongHieu;
            }
            int j = 0;
            foreach (var item2 in model.MatHangs.Where(x => x.listSPs.MaThuongHieu == MaLoai).ToList())
            {
                model.MatHangs[j] = item2; ;
                j++;
            }
            ViewBag.TH = model.MatHangs;
            int i = 0;
            HttpContext.Session.SetInt32("Ten", i);





            PagedList<MatHangHome> models = new PagedList<MatHangHome>(model.MatHangs.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.list = models;
            return View(model);
        }

        public IActionResult ThuongHieu(int MaTH)
        {

            HomeVM model = new HomeVM();
            var listcate = _context.ThuongHieus.AsNoTracking().ToList();
            ViewBag.listcate = listcate;
            var listSP = _context.MatHangs.AsNoTracking()
                .Where(x => x.DangDuocBan == true)
                .Where(x=>x.MaThuongHieu==MaTH)
                .ToList();
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

                if (HttpContext.Session.GetInt32("Ten") != null)
                {

                    ViewBag.Id = HttpContext.Session.GetInt32("Ten");
                }
                else
                {
                    ViewBag.Id = 0;
                }


            }
            
            HttpContext.Session.SetInt32("IDSP", MaTH);
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
            if (MaLoai == null)
            {
                MaLoai = listcate[0].MaThuongHieu;
            }
            int j = 0;
            foreach (var item2 in model.MatHangs.Where(x => x.listSPs.MaThuongHieu == MaLoai).ToList())
            {
                model.MatHangs[j] = item2; ;
                j++;
            }
            ViewBag.TH = model.MatHangs;
            int i = 0;
            HttpContext.Session.SetInt32("Ten", i);
            


 
           
            PagedList<MatHangHome> models = new PagedList<MatHangHome>(model.MatHangs.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.list = models;
            return View(model);
        }


        public IActionResult Timkiem(HomeVM timkiem)
        {
            var listcate = _context.ThuongHieus.AsNoTracking().ToList();
            ViewBag.listcate = listcate;
            HomeVM model = new HomeVM();
            List <MatHang> listSP=null;
            if (!String.IsNullOrEmpty(timkiem.key))
            {
                timkiem.key = timkiem.key.ToLower();
                listSP = _context.MatHangs.Where(b => b.TenMatHang.ToLower().Contains(timkiem.key)).ToList();
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


            }
            int i = 0;
            HttpContext.Session.SetInt32("Ten", i);
            return View(model);
        }
            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();//new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        }
    }
}
