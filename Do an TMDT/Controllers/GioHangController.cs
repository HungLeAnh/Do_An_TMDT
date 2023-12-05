using AspNetCoreHero.ToastNotification.Abstractions;
using Do_an_TMDT.Models;
using Do_an_TMDT.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Do_an_TMDT.Controllers
{
    public class GioHangController : Controller
    {
        private readonly WEBBANGIAYContext _context;
        public INotyfService _notyfService { get; }

        public GioHangController(WEBBANGIAYContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }


        public async Task<IActionResult> AddCart(int id)
        {
            CartVM cart = new CartVM();
            List<itemcart> itemcarts = new List<itemcart>();
            var listSP = _context.MatHangs
                .AsNoTracking().Where(x => x.DangDuocBan == true)
                .ToList();
            var listanh = _context.MatHangAnhs
                .AsNoTracking()
                .ToList();
            var listTH = _context.ThuongHieus
                .AsNoTracking()
                .ToList();
            int idgh = (int)HttpContext.Session.GetInt32("GH");
            var listGH = _context.ChiTietGioHangs.Where(x => x.MaGioHang == idgh)
               .AsNoTracking()
               .ToList();
            foreach (var item in listGH)
            {
                itemcart it = new itemcart();
                it.CT_GH = item;
                var SP = listSP.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                it.SanPham = SP[0];
                foreach (var item_anh in listanh)
                {
                    it.MatHangAnhs = listanh.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                }
                itemcarts.Add(it);
            }
            cart.item = itemcarts;
            int MaSp;
            if (HttpContext.Session.GetInt32("IDSP") != null)
            {
                MaSp = (int)HttpContext.Session.GetInt32("IDSP");
            }
            else {
                MaSp = id;
            }

            ViewBag.Id = HttpContext.Session.GetInt32("Ten");
            if (MaSp != 0) { id = MaSp; }
            var list = listGH.Where(x => x.MaMatHang == id).ToList();
            //kiểm tra mặt hàng và mã gio hàng có chưa
            var listCheck = listGH.Where(x => x.MaGioHang == idgh).ToList();
            var listCheck2 = listCheck.Where(x => x.MaMatHang == id).ToList();
            var list2 = listGH.Where(x => x.MaMatHang == id).ToList();
            var mathang = listSP.Where(x => x.MaMatHang == id).ToList();
            int sl;
            if (HttpContext.Session.GetInt32("sl") != null&& HttpContext.Session.GetInt32("sl") != 0)
            {
                sl = (int)HttpContext.Session.GetInt32("sl");
            }
            else
            {
                sl = 1;
            }
            
            if (listCheck2.Count == 0 )
            {
                ChiTietGioHang tc = new ChiTietGioHang
                {
                    MaGioHang = idgh,
                    MaMatHang = id,
                    SoLuong = sl,
                    Gia = (int)mathang[0].GiaBan,
                };
               
                _context.Add(tc);
                await _context.SaveChangesAsync();
                CartVM cartNew = new CartVM();
                List<itemcart> itemcartsNew = new List<itemcart>();
                var listGHNew = _context.ChiTietGioHangs.Where(x => x.MaGioHang == idgh)
               .AsNoTracking()
               .ToList();
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
                HttpContext.Session.SetInt32("sl", 0);
                HttpContext.Session.SetInt32("IDSP", 0);
                return RedirectToAction("ViewCart");
            }
            else
            {
                ChiTietGioHang tc = new ChiTietGioHang
                {
                    MaGioHang = idgh,
                    MaMatHang = id,
                    SoLuong = list[0].SoLuong + sl,
                    Gia = (int)mathang[0].GiaBan
                };
                ViewBag.giohang = listGH;
                _context.Update(tc);
                await _context.SaveChangesAsync();

                CartVM cartNew = new CartVM();
                List<itemcart> itemcartsNew = new List<itemcart>();
                var listGHNew = _context.ChiTietGioHangs.Where(x => x.MaGioHang == idgh)
               .AsNoTracking()
               .ToList();
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
                HttpContext.Session.SetInt32("sl", 0);
                HttpContext.Session.SetInt32("thanhtien", thanhtien);
                HttpContext.Session.SetInt32("IDSP", 0);
                return RedirectToAction("ViewCart");
            }
        }
        public async Task<IActionResult> DeleteCart(int id)
        {
            CartVM cart = new CartVM();
            List<itemcart> itemcarts = new List<itemcart>();
            var listSP = _context.MatHangs
                .AsNoTracking().Where(x => x.DangDuocBan == true)
                .ToList();
            var listanh = _context.MatHangAnhs
                .AsNoTracking()
                .ToList();
            var listTH = _context.ThuongHieus
                .AsNoTracking()
                .ToList();
            int idgh = (int)HttpContext.Session.GetInt32("GH");
            var listGH = _context.ChiTietGioHangs.Where(x => x.MaGioHang == idgh)
               .AsNoTracking()
               .ToList();
            ChiTietGioHang item = listGH.SingleOrDefault(p => p.MaMatHang == id);
            if (item != null)
            {
                _context.Remove(item);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewCart");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCart(int productID, int amount)
        {
            //Lay gio hang ra de xu ly
            ViewBag.Id = HttpContext.Session.GetInt32("Ten");
            int idgh = (int)HttpContext.Session.GetInt32("GH");
            var listCT = _context.ChiTietGioHangs.Where(x=>x.MaGioHang==idgh).ToList();
            try
            {
                if (listCT != null)
                {
                    var item = listCT.SingleOrDefault(p => p.MaMatHang== productID);
                    if (item != null && amount!=0)// da co -> cap nhat so luong
                    {
                        ChiTietGioHang ct = new ChiTietGioHang
                        {
                            MaGioHang=idgh,
                            MaMatHang=productID,
                            SoLuong=amount,
                            Gia=item.Gia

                        };

                        _context.Update(ct);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("ViewCart");

                    }

                    return RedirectToAction("ViewCart");



                }

                return RedirectToAction("ViewCart");

            }
            catch
            {
                return RedirectToAction("ViewCart");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCart([Bind("SoLuong")] HomeVM sl)
        {
            ViewBag.Id = HttpContext.Session.GetInt32("Ten");
            int idgh = (int)HttpContext.Session.GetInt32("GH");
            var listSP = _context.MatHangs
                .AsNoTracking().Where(x => x.DangDuocBan == true)
                .ToList();
            var listanh = _context.MatHangAnhs
               .AsNoTracking()
               .ToList();
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
            return RedirectToAction("ThanhToan","GioHang");

        }
        // GET: DonHangs
        public IActionResult ThanhToan()
        {
            var taikhoanID = HttpContext.Session.GetInt32("Ten");
            var khachhang = _context.NguoiDungs.AsNoTracking().Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID)).ToList();
            ViewBag.TenND = khachhang[0].TenNguoiDung;
            ViewBag.SDT = khachhang[0].Sdt;
            ViewBag.Email = khachhang[0].Email;
           
             if (HttpContext.Session.GetInt32("Ten") != null)
                {

                    ViewBag.Id = HttpContext.Session.GetInt32("Ten");
             }
             else
                {
                    ViewBag.Id = 0;
                }


           
            ViewData["DiaChi"] = new SelectList(_context.NguoiDungDiaChis, "DiaChi", "DiaChi");
            int idgh = (int)HttpContext.Session.GetInt32("GH");
            var listSP = _context.MatHangs
                .AsNoTracking().Where(x => x.DangDuocBan == true)
                .ToList();
            var listanh = _context.MatHangAnhs
               .AsNoTracking()
               .ToList();
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
            HttpContext.Session.SetInt32("thanhtien",thanhtien);
            var loi = HttpContext.Session.GetString("loi");
            if (loi != null)
            {
                ViewBag.error = loi;
            }
            HttpContext.Session.Remove("loi");
            return View();
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
            int idgh = (int)HttpContext.Session.GetInt32("GH");
            var listSP = _context.MatHangs
                .AsNoTracking().Where(x => x.DangDuocBan == true)
                .ToList();
            var listanh = _context.MatHangAnhs
               .AsNoTracking()
               .ToList();
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
            if (sl.DiaChi != null && sl.TenNguoiNhan != null && sl.SDT != null)
            {
                DonHang donhang = new DonHang
                {
                    MaNguoiDung = Convert.ToInt32(taikhoanID),
                    DiaChi = sl.DiaChi,
                    Sdt = sl.SDT,
                    TenNguoiNhan = sl.TenNguoiNhan,
                    TinhTrang = "Chưa xác nhận",
                    DaThanhToan = false,
                    TongTien = thanhtien,
                    NgayXuatDonHang = DateTime.Now
                };
                ViewBag.giohang = cartNew;
                _context.Add(donhang);
                await _context.SaveChangesAsync();
                var curdonhang = _context.DonHangs.Where(x => x == donhang).FirstOrDefault();
                var listsp_donhang = _context.ChiTietDonHangs.Where(x => x.MaDonHang == curdonhang.MaDonHang).ToList();
                foreach (var item in itemcartsNew)
                {
                    if (item.SanPham.SoLuong - item.CT_GH.SoLuong < 0)
                    {
                        ViewBag.eror = item.SanPham.TenMatHang + " Còn lại " + item.SanPham.SoLuong;

                        ViewData["DiaChi"] = new SelectList(_context.NguoiDungDiaChis, "DiaChi", "DiaChi");
                        ViewBag.thanhtien = thanhtien;
                        return View();
                    }
                    MatHang mathang = new MatHang
                    {
                        MaMatHang = item.SanPham.MaMatHang,
                        TenMatHang = item.SanPham.TenMatHang,
                        GiaBan = item.SanPham.GiaBan,
                        DangDuocBan = true,
                        SoSao = item.SanPham.SoSao,
                        SoLuong = item.SanPham.SoLuong - item.CT_GH.SoLuong,
                        SoLuongDaBan = item.SanPham.SoLuongDaBan + item.CT_GH.SoLuong,
                        MoTa = item.SanPham.MoTa,
                        DangDuocHienThi = true,
                        MaNhaCungCap = item.SanPham.MaNhaCungCap,
                        MaThuongHieu = item.SanPham.MaThuongHieu,
                        MaDanhMuc = item.SanPham.MaDanhMuc,
                        MaKichCo = item.SanPham.MaKichCo,
                        MaMauSac = item.SanPham.MaMauSac
                    };

                    _context.Update(mathang);
                    await _context.SaveChangesAsync();
                    ChiTietDonHang ctdonhang = new ChiTietDonHang
                    {
                        MaDonHang = curdonhang.MaDonHang,
                        MaMatHang = item.SanPham.MaMatHang,
                        Gia = item.SanPham.GiaBan,
                        SoLuong = item.CT_GH.SoLuong
                    };
                    _context.Update(ctdonhang);
                    await _context.SaveChangesAsync();



                }

                _notyfService.Success("Thanh toán thành công!");
                var mess = new MimeMessage();
                mess.From.Add(new MailboxAddress("Đơn Hàng:#" + donhang.MaDonHang, "20110675@student.hcmute.edu.vn"));
                mess.To.Add(new MailboxAddress("Đơn Hàng", khachhang[0].Email));
                mess.Subject = "Đơn hàng của bạn";
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h1>Đơn hàng:#" + donhang.MaDonHang + "</h1>" + "<br><h3>Tên Người dùng:</h3>" + khachhang[0].TenNguoiDung + "<br><h3>Số điện thoại:</h3>" + khachhang[0].Sdt + "<br><h3>Sản phẩm:<h3>" + "<br><h3>Địa Chỉ:<h3>" + sl.DiaChi + "<br><h3>Tổng tiền:<h3>" + thanhtien;
                mess.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {

                    client.Connect("smtp.elasticemail.com", 2525, false);
                    client.Authenticate("20110675@student.hcmute.edu.vn", "9974367BB96ED623DDE5482064AB01BE47AE");
                    client.Send(mess);
                    client.Disconnect(true);

                }
            }
            else
            {
                var loi = "Vui Lòng NHập Đầy Đủ Thông Tin !";
                HttpContext.Session.SetString("loi", loi);

                return RedirectToAction("ThanhToan");
            }
                return RedirectToAction("Loadsanpham", "NguoiDungs");
            

        }

        public IActionResult ViewCart(int id)
        {
             ViewBag.Id = HttpContext.Session.GetInt32("Ten");
            int idgh = (int)HttpContext.Session.GetInt32("GH");
            var listSP = _context.MatHangs
                .AsNoTracking().Where(x => x.DangDuocBan == true)
                .ToList();
            var listanh = _context.MatHangAnhs
               .AsNoTracking()
               .ToList();
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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewCart([Bind("SoLuong","DiaChi")] HomeVM sl)
        {
            
            return RedirectToAction("ThanhToan");
                

        }
    }
}
