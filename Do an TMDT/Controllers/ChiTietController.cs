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
    public class ChiTietController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public ChiTietController(WEBBANGIAYContext context)
        {
            _context = context;
        }

        // GET: ThuongHieux
        public IActionResult ChiTiet(int MaSp)
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
            foreach (var item2 in model.MatHangs.Where(x => x.listSPs.MaMatHang == MaSp).ToList())
            {
                model.MatHangs[0] = item2;
            }
            HttpContext.Session.SetInt32("IDSP", MaSp);
            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChiTiet( [Bind("SoLuong")] HomeVM sl, String id)
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

           int MaSp=(int)HttpContext.Session.GetInt32("IDSP");
            foreach (var item2 in model.MatHangs.Where(x => x.listSPs.MaMatHang == MaSp).ToList())
            {
                model.MatHangs[0] = item2;
            }
            if (sl.SoLuong <= model.MatHangs[0].listSPs.SoLuong)
            {
                if (sl.SoLuong > 0)
                {
                    if (id == "them")
                    {
                        HttpContext.Session.SetInt32("sl", sl.SoLuong);

                        return RedirectToAction("AddCart", "GioHang");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("sl", sl.SoLuong);
                        return RedirectToAction("ThanhToan", "DonHangs");
                    }
                }
                else
                {
                    ViewBag.eror = "Số lượng sản phẩm phải lớn hơn không!";
                    return View(model);
                }
            }
            else{
                ViewBag.eror = "Số lượng sản phẩm lớn hơn trong kho";
                return View(model);
            }

        }
    }
}
