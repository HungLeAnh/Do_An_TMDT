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
            int idgh = (int)HttpContext.Session.GetInt32("GH");
            var listGHNew = _context.ChiTietGioHangs.Where(x => x.MaGioHang == idgh)
                  .AsNoTracking()
                  .ToList();
            CartVM cartNew = new CartVM();
            List<itemcart> itemcartsNew = new List<itemcart>();
            int thanhtien = 0;
            foreach (var item in listGHNew)
            {
                itemcart it = new itemcart();
                it.CT_GH = item;
                var SP = listSP.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                it.SanPham = SP[0];
                foreach (var item_anh in listanh)
                {
                    it.MatHangAnhs = listanh.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                }
                itemcartsNew.Add(it);
                it.tong = (int)(SP[0].GiaBan * item.SoLuong);
                thanhtien += it.tong;
            }
            cartNew.item = itemcartsNew;
            ViewBag.thanhtien = thanhtien;
            ViewBag.giohang = cartNew;
            HttpContext.Session.SetInt32("thanhtien", thanhtien);
            HttpContext.Session.SetInt32("IDSP", 0);
            return View();
        }

        public IActionResult ThuongHieu(int MaTH)
        {

            HomeVM model = new HomeVM();
            var listcate = _context.ThuongHieus.AsNoTracking().ToList();
            ViewBag.listcate = listcate;
            var listSP = _context.MatHangs.AsNoTracking()
                .Where(x => x.DangDuocBan == true)
                .Where(x => x.MaThuongHieu == MaTH)
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
                int i = (int)HttpContext.Session.GetInt32("Ten");
                ViewBag.Id = HttpContext.Session.GetInt32("Ten");


            }


            return View(model);

        }
          
        public IActionResult dangky()
        {
            var listLND = _context.LoaiNguoiDungs.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult dangky([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
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

        public IActionResult dangnhap()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult dangnhap([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {
            int i = 0;

            var listLoaiNguoidung = _context.LoaiNguoiDungs.AsNoTracking().ToList();
            var list = _context.NguoiDungs.AsNoTracking().ToList();
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
                            return RedirectToAction("Loadsanpham");
                        }
                        else if (x == MaLoaiNguoiDung[2])
                        {
                            int id = item.MaNguoiDung;

                            HttpContext.Session.SetInt32("Ten", item.MaNguoiDung);
                            return RedirectToAction("", "Home", new { area = "Admin" });
                        }
                        else
                        {
                            int id = item.MaNguoiDung;
                            HttpContext.Session.SetInt32("Ten", item.MaNguoiDung);
                            return RedirectToAction("", "Home", new { id = id, area = "Shipper" });
                        }

                    }
                    ViewBag.mess = " Mật khẩu không đúng";

                }
                ViewBag.mess = " Tên đăng nhập không đúng";


            }

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
                    return RedirectToAction(nameof(dangnhap));
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

                return RedirectToAction(nameof(dangnhap));

            }
            else
            {
                ViewBag.mess = "Email không chính xác";
                return View();
            }


            return View();

        }
        private bool NguoiDungExists(int id)
        {
            return _context.NguoiDungs.Any(e => e.MaNguoiDung == id);
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
