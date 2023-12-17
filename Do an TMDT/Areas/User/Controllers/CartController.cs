using AspNetCoreHero.ToastNotification.Abstractions;
using Do_an_CCNPMM.Models;
using Do_an_CCNPMM.ViewModels;
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

namespace Do_an_CCNPMM.Areas.User.Controllers
{
    [Area("User")]
    public class CartController : Controller
    {
        private readonly WEBBANGIAYContext _context;
        public INotyfService _notyfService { get; }

        public CartController(WEBBANGIAYContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
/*        public async Task<IActionResult> AddCart(int? id)
        {
            int sl = 1;
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
            else
            {
                MaSp = id.GetValueOrDefault();
            }

            ViewBag.Id = HttpContext.Session.GetInt32("Ten");
            if (MaSp != 0) { id = MaSp; }
            var list = listGH.Where(x => x.MaMatHang == id).ToList();
            //kiểm tra mặt hàng và mã gio hàng có chưa
            var listCheck = listGH.Where(x => x.MaGioHang == idgh).ToList();
            var listCheck2 = listCheck.Where(x => x.MaMatHang == id).ToList();
            var list2 = listGH.Where(x => x.MaMatHang == id).ToList();
            var mathang = listSP.Where(x => x.MaMatHang == id).ToList();

            if (listCheck2.Count == 0)
            {
                ChiTietGioHang tc = new ChiTietGioHang
                {
                    MaGioHang = idgh,
                    MaMatHang = id.GetValueOrDefault(),
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
                long thanhtien = 0;
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
                HttpContext.Session.SetString("thanhtien", thanhtien.ToString());
                HttpContext.Session.SetInt32("sl", 0);
                HttpContext.Session.SetInt32("IDSP", 0);
                _notyfService.Success("Thêm vào giỏ hàng thành công");
                return RedirectToAction("Index");
            }
            else
            {
                ChiTietGioHang tc = new ChiTietGioHang
                {
                    MaGioHang = idgh,
                    MaMatHang = id.GetValueOrDefault(),
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
                long thanhtien = 0;
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
                HttpContext.Session.SetString("thanhtien", thanhtien.ToString());
                HttpContext.Session.SetInt32("IDSP", 0);
                _notyfService.Success("Thêm vào giỏ hàng thành công");
                return RedirectToAction("Index");
            }
        }*/
        public async Task<IActionResult> DeleteCart(int id, string siteBackURL="Index")
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

            return RedirectToAction(siteBackURL);
        }

        [HttpPost]
        public IActionResult AddCart(int? id, int? sl)
        {
            string message="";
            sl = sl.HasValue ? sl.Value : 0;
            if (sl <= 0)
            {
                message = "Số lượng không được bé hơn 1";
                return Json(new { Message = message });
            }
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
            else
            {
                MaSp = id.GetValueOrDefault();
            }

            ViewBag.Id = HttpContext.Session.GetInt32("Ten");
            if (MaSp != 0) { id = MaSp; }
            var list = listGH.Where(x => x.MaMatHang == id).ToList();
            //kiểm tra mặt hàng và mã gio hàng có chưa
            var listCheck = listGH.Where(x => x.MaGioHang == idgh).ToList();
            var listCheck2 = listCheck.Where(x => x.MaMatHang == id).ToList();
            var list2 = listGH.Where(x => x.MaMatHang == id).ToList();
            var mathang = listSP.Where(x => x.MaMatHang == id).ToList();

            var sanpham = _context.MatHangs.Where(x => x.MaMatHang == id).FirstOrDefault();

            if (listCheck2.Count == 0)
            {
                if (sanpham.SoLuong < sl)
                {
                    message = "Không đủ số lượng sản phẩm trong kho";
                    return Json(new { Message = message });

                }
                ChiTietGioHang tc = new ChiTietGioHang
                {
                    MaGioHang = idgh,
                    MaMatHang = id.GetValueOrDefault(),
                    SoLuong = sl.Value,
                    Gia = (int)mathang[0].GiaBan,
                };

                _context.Add(tc);
                _context.SaveChanges();
                CartVM cartNew = new CartVM();
                List<itemcart> itemcartsNew = new List<itemcart>();
                var listGHNew = _context.ChiTietGioHangs.Where(x => x.MaGioHang == idgh)
                .AsNoTracking()
                .ToList();
                long thanhtien = 0;
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
                HttpContext.Session.SetString("thanhtien", thanhtien.ToString());
                HttpContext.Session.SetInt32("sl", 0);
                HttpContext.Session.SetInt32("IDSP", 0);
                message = "Thêm vào giỏ hàng thành công";
                return Json(new { Message = message });
            }
            else
            {
                if (sanpham.SoLuong < list[0].SoLuong + sl)
                {
                    message = "Không đủ số lượng sản phẩm trong kho";
                    return Json(new { Message = message });
                }
                ChiTietGioHang tc = new ChiTietGioHang
                {
                    MaGioHang = idgh,
                    MaMatHang = id.GetValueOrDefault(),
                    SoLuong = list[0].SoLuong + sl.Value,
                    Gia = (int)mathang[0].GiaBan
                };
                ViewBag.giohang = listGH;
                _context.Update(tc);
                _context.SaveChanges();

                CartVM cartNew = new CartVM();
                List<itemcart> itemcartsNew = new List<itemcart>();
                var listGHNew = _context.ChiTietGioHangs.Where(x => x.MaGioHang == idgh)
                .AsNoTracking()
                .ToList();
                long thanhtien = 0;
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
                HttpContext.Session.SetString("thanhtien", thanhtien.ToString());
                HttpContext.Session.SetInt32("IDSP", 0);

                message = "Thêm vào giỏ hàng thành công";
                return Json(new { Message = message });
            }
            
        }
        public async Task<IActionResult> UpdateCart(int productID, int amount,int update, string siteBackURL="Index")
        {
            //Lay gio hang ra de xu ly
            ViewBag.Id = HttpContext.Session.GetInt32("Ten");
            int idgh = (int)HttpContext.Session.GetInt32("GH");
            var listCT = _context.ChiTietGioHangs.Where(x => x.MaGioHang == idgh);
            var sanpham = _context.MatHangs.Where(x => x.MaMatHang == productID).FirstOrDefault();

            if(sanpham.SoLuong < amount + update)
            {
                _notyfService.Error("Không đủ số lượng sản phẩm trong kho");
                return RedirectToAction("Index");

            }
            try
            {
                if (listCT != null)
                {
                    var item = listCT.SingleOrDefault(p => p.MaMatHang == productID);
                    if (item != null && amount != 0)// da co -> cap nhat so luong
                    {
                        if(amount + update == 0)
                        {
                            _context.Remove(item);
                        }
                        else
                        {
                            item.SoLuong = amount + update;

                            _context.Update(item);
                        }
                        await _context.SaveChangesAsync();
                        return RedirectToAction(siteBackURL);

                    }

                    return RedirectToAction("Index");



                }

                return RedirectToAction("Index");

            }
            catch
            {
                return RedirectToAction("Index");

            }
        }
        // GET: DonHangs
        public IActionResult CheckOut()
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
            long thanhtien = 0;
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
            HttpContext.Session.SetString("thanhtien", thanhtien.ToString());
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(HomeVM sl)
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

            List<itemcart> itemcartsNew = new List<itemcart>();
            long thanhtien = 0;
            foreach (var item in listGHNew)
            {
                itemcart it = new itemcart();
                it.CT_GH = item;
                var SP = listSP.Where(x => x.MaMatHang == item.MaMatHang).ToList();
                it.SanPham = SP[0];
                it.tong = (int)(SP[0].GiaBan * item.SoLuong);
                itemcartsNew.Add(it);
                thanhtien += it.tong;
            }
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
                _context.Add(donhang);
                await _context.SaveChangesAsync();

                var curdonhang = _context.DonHangs.Where(x => x == donhang).FirstOrDefault();
                foreach (var item in itemcartsNew)
                {
                    if (item.SanPham.SoLuong - item.CT_GH.SoLuong < 0)
                    {
                        ViewBag.eror = item.SanPham.TenMatHang + " Còn lại " + item.SanPham.SoLuong;

                        ViewData["DiaChi"] = new SelectList(_context.NguoiDungDiaChis, "DiaChi", "DiaChi");
                        ViewBag.thanhtien = thanhtien;
                        _notyfService.Error(item.SanPham.TenMatHang + " Còn lại " + item.SanPham.SoLuong);
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
                foreach(var item in listGHNew)
                {
                    _context.Remove(item);
                    await _context.SaveChangesAsync();

                }
                string ctdh = "<table class=\"inventory\" style=\"border:1px solid black;\">\r\n\t\t\t\t<thead>\r\n\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t<th style=\"border:1px solid black;\"><span contenteditable>Sản phẩm</span></th>\r\n\t\t\t\t\t\t<th style=\"border:1px solid black;\"><span contenteditable>Số lượng</span></th>\r\n\t\t\t\t\t\t<th style=\"border:1px solid black;\"><span contenteditable>Giá</span></th>\r\n\t\t\t\t\t</tr>\r\n\t\t\t\t</thead>\r\n\t\t\t\t<tbody>";
                List<ChiTietDonHang> temp = _context.ChiTietDonHangs.Where(x => x.MaDonHang.Equals(curdonhang.MaDonHang)).ToList();
                temp.ForEach(x =>
                {
                    var temp = _context.MatHangs.Where(a => a.MaMatHang.Equals(x.MaMatHang)).FirstOrDefault();
                    ctdh += "<tr><td style=\"border:1px solid black;\">" + temp.TenMatHang + "</td>" + "<td style=\"border:1px solid black;\">" + x.SoLuong + "</td>" + "<td style=\"border:1px solid black;\">" + x.Gia + "</td></tr>";
                });
                ctdh += "</tbody>\r\n\t\t\t</table>";

                var mess = new MimeMessage();
                mess.From.Add(new MailboxAddress("Đơn Hàng:#" + donhang.MaDonHang, "20110675@student.hcmute.edu.vn"));
                mess.To.Add(new MailboxAddress("Đơn Hàng", khachhang[0].Email));
                mess.Subject = "Đơn hàng của bạn";
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h1>Đơn hàng:#" + donhang.MaDonHang + "</h1>" + "<br><h3>Tên Người dùng:</h3>" + khachhang[0].TenNguoiDung + "<br><h3>Số điện thoại:</h3>" + khachhang[0].Sdt + "<br><h3>Sản phẩm:<h3>" + ctdh + "<br><h3>Địa Chỉ:<h3>" + sl.DiaChi + "<br><h3>Tổng tiền:<h3>" + thanhtien;
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
                _notyfService.Error(loi);
                return RedirectToAction("CheckOut");
            }
            _notyfService.Success("Đặt hàng thành công");
            return RedirectToAction("Index");


        }

        public IActionResult Index()
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
            long thanhtien = 0;
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
            HttpContext.Session.SetString("thanhtien", thanhtien.ToString());
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Index([Bind("SoLuong", "DiaChi")] HomeVM sl)
        {

            return RedirectToAction("CheckOut");


        }
    }
}
