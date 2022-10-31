using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Do_an_TMDT.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;
using System.Text;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Do_an_TMDT.Controllers
{
    public class NguoiDungsController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public NguoiDungsController( WEBBANGIAYContext context)
        {
           
            _context = context;

        }
        [HttpGet]
        public ActionResult Login(NguoiDung user)
        {
            var x = _context.NguoiDungs.Where(y => y.Email == user.Email && y.MatKhauHash == user.MatKhauHash);
            if (x == null)
            {
                return View("Dangky");
            }
            else
            {
                return View("Login");
            }

        }
        [HttpPost]
        public ActionResult Dangky(NguoiDung user)
        {
            var x = _context.NguoiDungs.Where(y => y.Email == user.Email && y.MatKhauHash == user.MatKhauHash);
            if (x == null)
            {
                return View("Dangky");
            }
            else
            {
                return View("Login");
            }

        }


    }
}

