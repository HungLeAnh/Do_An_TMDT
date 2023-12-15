using Do_an_CCNPMM.Extension;
using Do_an_CCNPMM.Helpper;
using Do_an_CCNPMM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Threading.Tasks;
using System;
using AspNetCoreHero.ToastNotification.Abstractions;
using Do_an_CCNPMM.Areas.Admin.Controllers;
using Microsoft.Extensions.Logging;
using System.Linq;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Do_an_CCNPMM.Areas.User.Controllers
{
    [Area("User")]
    [AllowAnonymous]
    public class UserLoginController : Controller
    {
        private readonly ILogger<AdminLoginController> _logger;
        private readonly WEBBANGIAYContext _context;
        public INotyfService _notyfService { get; }

        public UserLoginController(WEBBANGIAYContext context, ILogger<AdminLoginController> logger, INotyfService notyfService)
        {
            _context = context;
            _logger = logger;
            _notyfService = notyfService;
        }

        [Route("User/login", Name = "UserLogin")]
        public IActionResult Login()
        {
            bool isLoggedIn = (ClaimsPrincipal.Current != null) && ClaimsPrincipal.Current.Identity.IsAuthenticated;
            if (isLoggedIn)
                return RedirectToAction("Index");
            return View();
        }


        [HttpPost]
        [Route("User/login", Name = "UserLogin")]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {
            int i = 0;

            var listLoaiNguoidung = _context.LoaiNguoiDungs.AsNoTracking().ToList();
            var list = _context.NguoiDungs.Include(x => x.MaLoaiNguoiDungNavigation).AsNoTracking().ToList();
            string[] MaLoaiNguoiDung = new string[listLoaiNguoidung.Count()];
            foreach (var item in listLoaiNguoidung)
            {
                MaLoaiNguoiDung[i] = item.MaLoaiNguoiDung;
                i++;
            }
            foreach (var item in list)
            {
                if (nguoiDung.Email == item.Email || nguoiDung.Email == item.TenDangNhap)
                {
                    string pass = (nguoiDung.MatKhauHash + item.Salt.Trim()).ToMD5();
                    if (pass == item.MatKhauHash)
                    {
                        String x = item.MaLoaiNguoiDung;
                        if (x == MaLoaiNguoiDung[0])
                        {
                            int id = item.MaNguoiDung;
                            var ctng = _context.GioHangs.Where(x => x.MaNguoiDung == item.MaNguoiDung).ToList();
                            HttpContext.Session.SetInt32("Ten", item.MaNguoiDung);
                            HttpContext.Session.SetInt32("GH", ctng[0].MaGioHang);
                            var claims = new List<Claim>() {
                                new Claim(ClaimTypes.Sid, item.MaNguoiDung.ToString()),
                                new Claim(ClaimTypes.Name, item.TenNguoiDung),
                                new Claim(ClaimTypes.Role, item.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung),
                                new Claim(ClaimTypes.UserData, ctng[0].MaGioHang.ToString()),
                            };

                            //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                            var principal = new ClaimsPrincipal(identity);
                            //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                            HttpContext.SignInAsync("UserLogin", principal, new AuthenticationProperties()
                            {
                                IsPersistent = false,
                            });
                            return RedirectToAction("", "Home", new { area = "User" });
                        }

                    }
                    ViewBag.mess = " Mật khẩu không đúng";

                }
                ViewBag.mess = " Tên đăng nhập không đúng";


            }

            return View();
        }
        public IActionResult Register()
        {
            var listLND = _context.LoaiNguoiDungs.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                bool x = true;
                string salt = Utilities.GetRandomKey();
                var listLoaiNguoidung = _context.LoaiNguoiDungs.AsNoTracking().ToList();
                NguoiDung khachhang = new NguoiDung
                {
                    MaLoaiNguoiDung = listLoaiNguoidung[0].MaLoaiNguoiDung,
                    TenDangNhap = nguoiDung.TenDangNhap,
                    TenNguoiDung = nguoiDung.TenNguoiDung,
                    Sdt = nguoiDung.Sdt.Trim().ToLower(),
                    Email = nguoiDung.Email.Trim().ToLower(),
                    MatKhauHash = (nguoiDung.MatKhauHash + salt.Trim()).ToMD5(),
                    Salt = salt
                };

                HttpContext.Session.SetString("MaLoaiNguoiDung", khachhang.MaLoaiNguoiDung);
                HttpContext.Session.SetString("TenDangNhap", khachhang.TenDangNhap);
                HttpContext.Session.SetString("TenNguoiDung", khachhang.TenNguoiDung);
                HttpContext.Session.SetString("SDT", khachhang.Sdt);
                HttpContext.Session.SetString("Email", khachhang.Email);
                HttpContext.Session.SetString("MK", khachhang.MatKhauHash);
                HttpContext.Session.SetString("Salt", khachhang.Salt);
                var list = _context.NguoiDungs.AsNoTracking().ToList();
                foreach (var item in list)
                {
                    if (khachhang.Email == item.Email)
                    {
                        ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
                        ViewBag.mess = "Email đã tồn tại";
                        x = false;
                        return View();
                    }
                    if (khachhang.Sdt == item.Sdt)
                    {
                        ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
                        ViewBag.mess = " SDT đã tồn tại";
                        x = false;
                        return View();
                    }
                    if (khachhang.TenDangNhap == item.TenDangNhap)
                    {
                        ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
                        ViewBag.mess = " Tên Đăng Nhập đã tồn tại";
                        x = false;
                        return View();
                    }
                }
                if (x == true)
                {
                    if (khachhang.Sdt.Length == 10)
                    {
                        Random ran = new Random();
                        int rad = ran.Next(100000, 999999);
                        HttpContext.Session.SetInt32("OTP", rad);


                        var mess = new MimeMessage();
                        mess.From.Add(new MailboxAddress("ND Shoes", "20110675@student.hcmute.edu.vn"));
                        mess.To.Add(new MailboxAddress("Xác Thực", nguoiDung.Email));
                        mess.Subject = "Xác Thực Email";
                        mess.Body = new TextPart("plain")
                        {
                            Text = "OTP:" + rad

                        };
                        using (var client = new SmtpClient())
                        {

                            client.Connect("smtp.elasticemail.com", 2525, false);
                            client.Authenticate("20110675@student.hcmute.edu.vn", "9974367BB96ED623DDE5482064AB01BE47AE");
                            client.Send(mess);
                            client.Disconnect(true);

                        }
                        return RedirectToAction(nameof(OTP));
                    }
                    else
                    {
                        ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
                        ViewBag.mess = " SDT không hợp lệ";
                        x = false;
                        return View();
                    }
                }
            }
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
            return View();
        }
        public IActionResult OTP()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OTP([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {
            var otp = HttpContext.Session.GetInt32("OTP");
            if (ModelState.IsValid)
            {


                if (nguoiDung.MaNguoiDung == otp)
                {
                    NguoiDung khachhang = new NguoiDung
                    {
                        MaLoaiNguoiDung = HttpContext.Session.GetString("MaLoaiNguoiDung"),
                        TenDangNhap = HttpContext.Session.GetString("TenDangNhap"),
                        TenNguoiDung = HttpContext.Session.GetString("TenNguoiDung"),
                        Sdt = HttpContext.Session.GetString("SDT"),
                        Email = HttpContext.Session.GetString("Email"),
                        MatKhauHash = HttpContext.Session.GetString("MK"),
                        Salt = HttpContext.Session.GetString("Salt")
                    };

                    _context.Add(khachhang);
                    await _context.SaveChangesAsync();
                    var list = _context.NguoiDungs.Where(x => x.Email == khachhang.Email).ToList();

                    GioHang gioHang = new GioHang
                    {
                        MaNguoiDung = list[0].MaNguoiDung,
                        MaGioHang = list[0].MaNguoiDung
                    };

                    _context.Add(gioHang);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Login));
                }


            }
            return View();

        }
        public IActionResult QuenMK()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuenMK([Bind("Email")] NguoiDung nguoiDung)
        {
            var ng = _context.NguoiDungs.Where(x => x.Email == nguoiDung.Email).FirstOrDefault();
            if (ng != null)
            {
                Random ran = new Random();
                int rad = ran.Next(100000, 999999);
                HttpContext.Session.SetInt32("OTP", rad);
                var mess = new MimeMessage();
                mess.From.Add(new MailboxAddress("ND Shoes", "20110675@student.hcmute.edu.vn"));
                mess.To.Add(new MailboxAddress("Xác Thực", nguoiDung.Email));
                mess.Subject = "Xác Thực Email";
                mess.Body = new TextPart("plain")
                {
                    Text = "Mật Khẩu mới:" + rad
                };
                using (var client = new SmtpClient())
                {

                    client.Connect("smtp.elasticemail.com", 2525, false);
                    client.Authenticate("20110675@student.hcmute.edu.vn", "9974367BB96ED623DDE5482064AB01BE47AE");
                    client.Send(mess);
                    client.Disconnect(true);
                }




                string salt = Utilities.GetRandomKey();



                ng.MatKhauHash = (rad + salt.Trim()).ToMD5();
                ng.Salt = salt;

                _context.NguoiDungs.Update(ng);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Login));

            }
            else
            {
                ViewBag.mess = "Email không chính xác";
                return View();
            }

        }
        private bool NguoiDungExists(int id)
        {
            return _context.NguoiDungs.Any(e => e.MaNguoiDung == id);
        }

        [Route("UserLogout", Name = "UserLogout")]
        public async Task<IActionResult> Logout()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync("UserLogin");
            //Redirect to home page    
            return LocalRedirect("/");
        }
    }
}
