using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Do_an_TMDT.ViewModels;
using Do_an_TMDT.Helpper;
using Do_an_TMDT.Extension;
using MimeKit;
using MailKit.Net.Smtp;

namespace Do_an_TMDT.Controllers
{
    public class NguoiDungsController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public NguoiDungsController(WEBBANGIAYContext context)
        {
            _context = context;
        }
        public IActionResult Cart(int MaLoai)
        {
            int Id = (int)HttpContext.Session.GetInt32("Ten");
            var list = _context.GioHangs.Where(x => x.MaNguoiDung == Id).ToList();
            int idGH = list[0].MaGioHang;
            var list_SP = _context.ChiTietGioHangs.Where(x => x.MaGioHang == idGH).ToList();

            return View();
        }
        public IActionResult AddCart(int MaSP)
        {
            var list_SP = _context.ChiTietGioHangs.Where(x => x.MaGioHang == MaSP).ToList();
            return View();
        }
            public IActionResult Loadsanpham()
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
            return View();
        }


        public IActionResult dangky()
        {
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> dangky([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                bool x = true;
                string salt = Utilities.GetRandomKey();
                var listLoaiNguoidung = _context.LoaiNguoiDungs.AsNoTracking().ToList();
                NguoiDung khachhang = new NguoiDung
                {
                    MaLoaiNguoiDung = nguoiDung.MaLoaiNguoiDung,
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
                        mess.From.Add(new MailboxAddress("Trần Bửu Quyến", "tranbuuquyen2002@gmail.com"));
                        mess.To.Add(new MailboxAddress("Xác Thực", nguoiDung.Email));
                        mess.Subject = "Xác Thực Email";
                        mess.Body = new TextPart("plain")
                        {
                            Text = "OTP:" + rad

                        };
                        using (var client = new SmtpClient())
                        {

                            client.Connect("smtp.gmail.com", 587, false);
                            client.Authenticate("tranbuuquyen2002@gmail.com", "hgaictvgopbceprr");
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
        public async Task<IActionResult> dangnhap([Bind("MaNguoiDung,MaLoaiNguoiDung,TenNguoiDung,AnhDaiDien,TenDangNhap,MatKhauHash,Salt,Email,Sdt,ViDienTu")] NguoiDung nguoiDung)
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

                            HttpContext.Session.SetInt32("Ten", item.MaNguoiDung);
                          
                            return RedirectToAction("Loadsanpham");
                        }
                        else if (x == MaLoaiNguoiDung[2])
                        {
                            int id = item.MaNguoiDung;

                            HttpContext.Session.SetInt32("Ten", item.MaNguoiDung);
                            return RedirectToAction("", "Home", new { area = "Admin" });
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
            var otp =HttpContext.Session.GetInt32("OTP");
            if (!ModelState.IsValid)
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
