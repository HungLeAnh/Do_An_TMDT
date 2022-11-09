using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;
using Microsoft.AspNetCore.Http;
using Do_an_TMDT.ViewModels;
using MimeKit;
using System.Net;
using MailKit.Net.Smtp;

namespace Do_an_TMDT.Controllers
{
    public class DonHangsController : Controller
    {
        private readonly WEBBANGIAYContext _context;

        public DonHangsController(WEBBANGIAYContext context)
        {
            _context = context;
        }

        // GET: DonHangs
        public IActionResult ThanhToan()
        {
            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
            ViewBag.TenND = khachhang[0].TenNguoiDung;
            ViewBag.SDT = khachhang[0].Sdt;
            ViewBag.Email = khachhang[0].Email;
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
            var MaSp = HttpContext.Session.GetInt32("IDSP");
            foreach (var item2 in model.MatHangs.Where(x => x.listSPs.MaMatHang == Convert.ToInt32(MaSp)).ToList())
            {
                model.MatHangs[0] = item2;
            }
            ViewData["DiaChi"] = new SelectList(_context.NguoiDungDiaChis, "DiaChi", "DiaChi");
            int sl = (int)HttpContext.Session.GetInt32("sl");
            ViewBag.Tong = sl * model.MatHangs[0].listSPs.GiaBan;
            ViewBag.ThanhTien = sl * model.MatHangs[0].listSPs.GiaBan + 800000;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThanhToan(HomeVM sl)
        {
            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
            ViewBag.TenND = khachhang[0].TenNguoiDung;
            ViewBag.SDT = khachhang[0].Sdt;
            ViewBag.Email = khachhang[0].Email;
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
            var MaSp = HttpContext.Session.GetInt32("IDSP");
            foreach (var item2 in model.MatHangs.Where(x => x.listSPs.MaMatHang == Convert.ToInt32(MaSp)).ToList())
            {
                model.MatHangs[0] = item2;
            }

            int sl1 = (int)HttpContext.Session.GetInt32("sl");
            ViewBag.Tong = sl1 * model.MatHangs[0].listSPs.GiaBan;
            ViewBag.ThanhTien = sl1 * model.MatHangs[0].listSPs.GiaBan + 800000;
            decimal tong= sl1 * model.MatHangs[0].listSPs.GiaBan + 800000;
            DonHang donhang = new DonHang
            {
                MaNguoiDung = Convert.ToInt32(taikhoanID),
                DiaChi = sl.DiaChi,
                Sdt = khachhang[0].Sdt,
                TinhTrang = "Đang Giao",
                DaThanhToan = true,
                TongTien = sl1 * model.MatHangs[0].listSPs.GiaBan + 80000
            };

            _context.Add(donhang);
          
                MatHang mathang = new MatHang
                {
                    MaMatHang = Convert.ToInt32(MaSp),
                    TenMatHang = model.MatHangs[0].listSPs.TenMatHang,
                    GiaBan = model.MatHangs[0].listSPs.GiaBan,
                    DangDuocBan = true,
                    SoSao = model.MatHangs[0].listSPs.SoSao,
                    SoLuong = model.MatHangs[0].listSPs.SoLuong - sl1,
                    SoLuongDaBan = model.MatHangs[0].listSPs.SoLuongDaBan + sl1,
                    MoTa = model.MatHangs[0].listSPs.MoTa,
                    DangDuocHienThi = true,
                    MaNhaCungCap = model.MatHangs[0].listSPs.MaNhaCungCap,
                    MaThuongHieu = model.MatHangs[0].listSPs.MaThuongHieu,
                    MaDanhMuc = model.MatHangs[0].listSPs.MaDanhMuc,
                    MaKichCo = model.MatHangs[0].listSPs.MaKichCo,
                    MaMauSac = model.MatHangs[0].listSPs.MaMauSac
                };

                _context.Update(mathang);
                await _context.SaveChangesAsync();
            var mess = new MimeMessage();
            mess.From.Add(new MailboxAddress("Đơn Hàng:#"+donhang.MaDonHang, "tranbuuquyen2002@gmail.com"));
            mess.To.Add(new MailboxAddress("Đơn Hàng", khachhang[0].Email));
            mess.Subject = "Đơn hàng của bạn";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<h1>Đơn hàng:#"+ donhang.MaDonHang+ "</h1>"+"<br><h3>Tên Người dùng:</h3>"+khachhang[0].TenNguoiDung + "<br><h3>Số điện thoại:</h3>" + khachhang[0].Sdt + "<br><h3>Sản phẩm:<h3>" + model.MatHangs[0].listSPs.TenMatHang + "<br><h3>Số Lượng:<h3>" + sl1+"<br><h3>Địa Chỉ:<h3>" + sl.DiaChi + "<br><h3>Tổng tiền:<h3>" + tong;
            mess.Body = bodyBuilder.ToMessageBody();
            
            using (var client = new SmtpClient())
            {

                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("tranbuuquyen2002@gmail.com", "hgaictvgopbceprr");
                client.Send(mess);
                client.Disconnect(true);

            }
            return RedirectToAction(nameof(BaoCao));
            

            

        }
        public IActionResult BaoCao()
        {
            

            return View();
        }
    }

}
