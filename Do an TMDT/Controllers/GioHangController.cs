using Do_an_TMDT.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Do_an_TMDT.Controllers
{
    public class GioHangController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public GioHangController(WEBBANGIAYContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
