using Do_an_CCNPMM.Models;
using Do_an_CCNPMM.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Do_an_CCNPMM.Areas.User.Controllers
{
    [Area("User")]
    public class ProductDetailController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public ProductDetailController(WEBBANGIAYContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? id)
        {

            HomeVM model = new HomeVM();
            var listSP = _context.MatHangs
                .Include(m => m.MaDanhMucNavigation)
                .Include(m => m.MaKichCoNavigation)
                .Include(m => m.MaMauSacNavigation)
                .Include(m => m.MaNhaCungCapNavigation)
                .Include(m => m.MaThuongHieuNavigation)
                .Include(m => m.MatHangAnhs)
                .AsNoTracking()
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
            foreach (var item2 in model.MatHangs.Where(x => x.listSPs.MaMatHang == id).ToList())
            {
                model.MatHangs[0] = item2;
            }
            HttpContext.Session.SetInt32("IDSP", id == null ? default(int) : id.Value);
            return View(model);

        }


       /* [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("SoLuong")] HomeVM sl, String id)
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

            int MaSp = (int)HttpContext.Session.GetInt32("IDSP");
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
            else
            {
                ViewBag.eror = "Số lượng sản phẩm lớn hơn trong kho";
                return View(model);
            }

        }*/
    }
}
