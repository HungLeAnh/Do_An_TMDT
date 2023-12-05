using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;
using Do_an_TMDT.ViewModels;

namespace Do_an_TMDT.Controllers
{
    public class MatHangController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public MatHangController(WEBBANGIAYContext context)
        {
            _context = context;
        }

        /*        public IActionResult Index()
                {
                    return View();
                }*/
        public IActionResult ChiTiet(int MaMatHang)
        {
            MaMatHang = 1;
            var viewModel = new ViewChiTietMatHangModel();
            viewModel.MatHangs = _context.MatHangs.Where(n => n.MaMatHang == MaMatHang).FirstOrDefault();
            if(viewModel.MatHangs != null)
            {
                return View(viewModel);

            }
            else
            {
                return View();
            }
        }
    }
}
