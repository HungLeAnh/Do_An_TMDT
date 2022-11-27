using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList.Core;
using System.Web;
using System.IO;
using Do_an_TMDT.Helpper;
using AspNetCoreHero.ToastNotification.Abstractions;
using Do_an_TMDT.ViewModels;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Http;
using static System.Net.WebRequestMethods;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;

namespace Do_an_TMDT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMatHangsController : Controller
    {
        private readonly WEBBANGIAYContext _context;
        public INotyfService _notyfService { get; }

        public AdminMatHangsController(WEBBANGIAYContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminMatHangs
        public IActionResult Index(int? page=1,int? maDanhMuc=0)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            List<MatHang> lsMatHangs = new List<MatHang>();   
            if (maDanhMuc != 0)
            {
                 lsMatHangs = _context.MatHangs
                    .Where(x=>x.MaDanhMuc == maDanhMuc && x.DangDuocHienThi==true)
                    .Include(m => m.MaDanhMucNavigation)
                    .Include(m => m.MaKichCoNavigation)
                    .Include(m => m.MaMauSacNavigation)
                    .Include(m => m.MaNhaCungCapNavigation)
                    .Include(m => m.MaThuongHieuNavigation)
                    .Include(m => m.MatHangAnhs)
                    .AsNoTracking()
                    .OrderByDescending(x => x.MaMatHang).ToList();

            }
            else
            {
                 lsMatHangs = _context.MatHangs
                    .Where(x=>x.DangDuocHienThi==true)
                    .Include(m => m.MaDanhMucNavigation)
                    .Include(m => m.MaKichCoNavigation)
                    .Include(m => m.MaMauSacNavigation)
                    .Include(m => m.MaNhaCungCapNavigation)
                    .Include(m => m.MaThuongHieuNavigation)
                    .Include(m => m.MatHangAnhs)
                    .AsNoTracking()
                    .OrderByDescending(x => x.MaMatHang).ToList();
            }
            PagedList<MatHang> models = new PagedList<MatHang>(lsMatHangs.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentMaDanhMuc = maDanhMuc;
            ViewData[nameof(DanhMuc)] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc",maDanhMuc);
            return View(models);
        }

        public IActionResult Filtter(int maDanhMuc = 0)
        {
            var url = $"/Admin/AdminMatHangs?MaDanhMuc={maDanhMuc}";
            if (maDanhMuc == 0)
            {
                url = $"/Admin/AdminMatHangs";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        // GET: Admin/AdminMatHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matHang = await _context.MatHangs
                .Include(m => m.MaDanhMucNavigation)
                .Include(m => m.MaKichCoNavigation)
                .Include(m => m.MaMauSacNavigation)
                .Include(m => m.MaNhaCungCapNavigation)
                .Include(m => m.MaThuongHieuNavigation)
                .Include(m => m.MatHangAnhs)
                .FirstOrDefaultAsync(m => m.MaMatHang == id);
            if (matHang == null)
            {
                return NotFound();
            }

            return View(matHang);
        }

        // GET: Admin/AdminMatHangs/Create
        [HttpGet]
        public IActionResult Create()
        {
            //IFormFile formFile = null;
            ViewData["DanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewData["NhaCungCap"] = new SelectList(_context.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap");
            ViewData["ThuongHieu"] = new SelectList(_context.ThuongHieus, "MaThuongHieu", "TenThuongHieu");
            return View();//formFile
        }
        // POST: Admin/AdminMatHangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaMatHang,TenMatHang,GiaBan,GiaNhap,DangDuocBan,SoSao,SoLuong,SoLuongDaBan,MoTa,MaNhaCungCap,MaThuongHieu,MaDanhMuc,")]MatHang matHang,int kichCo,string mauSac,IFormFile[] files)
        {
            matHang.SoLuongDaBan = 0;
            matHang.SoSao = 0;
            matHang.DangDuocHienThi = true;
            if(matHang.MoTa == null)
            {
                matHang.MoTa = "";
            }
            if (ModelState.IsValid)
            {
                var hasKichCo = _context.KichCos.Where(m => m.KichCo1 == kichCo).FirstOrDefault();
                var hasMauSac = _context.MauSacs.Where(m => m.TenMauSac == mauSac).FirstOrDefault();
                if ( hasKichCo != null )
                {
                    matHang.MaKichCo = hasKichCo.MaKichCo;
                }
                else
                {
                    KichCo newKichCo = new KichCo();
                    newKichCo.KichCo1 = kichCo;
                    _context.Add(newKichCo);
                    await _context.SaveChangesAsync();

                    matHang.MaKichCo = _context.KichCos.Where(m=>m.KichCo1==kichCo).FirstOrDefault().MaKichCo;
                }
                if (hasMauSac != null)
                {
                    matHang.MaMauSac = hasMauSac.MaMauSac;
                }
                else
                {
                    MauSac newMauSac = new MauSac();
                    newMauSac.TenMauSac = mauSac;
                    _context.Add(newMauSac);
                    await _context.SaveChangesAsync();
                    matHang.MaMauSac = _context.MauSacs.Where(m => m.TenMauSac == mauSac).FirstOrDefault().MaMauSac;

                }
                _context.Add(matHang);
                await _context.SaveChangesAsync();
              
                string tenMatHang = Utilities.ToTitleCase(matHang.TenMatHang);
                if (files != null)
                {
                    foreach (var file in files)
                    {

                        MatHangAnh matHangAnh = new MatHangAnh();
                        matHangAnh.MaMatHang = matHang.MaMatHang;
                        string extension = Path.GetExtension(file.FileName);
                        string image = Utilities.SEOUrl(tenMatHang) + DateTime.Now.Ticks + extension;
                        matHangAnh.Anh = await Utilities.UploadFile(file, @"products", image.ToLower());
                        if (string.IsNullOrEmpty(matHangAnh.Anh)) matHangAnh.Anh = "images/duphong.webp";

                        _context.Add(matHangAnh);
                    }

                }
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }

            ViewData["DanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc", matHang.MaDanhMuc);
            ViewData["KichCo"] = new SelectList(_context.KichCos, "MaKichCo", "KichCo1", matHang.MaKichCo);
            ViewData["MauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "TenMauSac", matHang.MaMauSac);
            ViewData["NhaCungCap"] = new SelectList(_context.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", matHang.MaNhaCungCap);
            ViewData["ThuongHieu"] = new SelectList(_context.ThuongHieus, "MaThuongHieu", "TenThuongHieu", matHang.MaThuongHieu);
            return View(matHang);
        }
        
        // GET: Admin/AdminMatHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matHang =  await _context.MatHangs
                            .Include(m => m.MatHangAnhs)
                            .Include(m=>m.MaMauSacNavigation)
                            .Include(m=>m.MaKichCoNavigation)
                            .Where(m =>m.MaMatHang == id).FirstOrDefaultAsync();             
            if (matHang == null)
            {
                return NotFound();
            }

            ViewData["DanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc", matHang.MaDanhMuc);
            ViewBag.KichCo =  matHang.MaKichCoNavigation.KichCo1;
            ViewBag.MauSac =  matHang.MaMauSacNavigation.TenMauSac;
            ViewData["NhaCungCap"] = new SelectList(_context.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", matHang.MaNhaCungCap);
            ViewData["ThuongHieu"] = new SelectList(_context.ThuongHieus, "MaThuongHieu", "TenThuongHieu", matHang.MaThuongHieu);
            ViewData["MatHangAnh"] = new SelectList(_context.MatHangAnhs, "MaAnh", "Anh", matHang.MatHangAnhs);
            return View(matHang);
        }

        // POST: Admin/AdminMatHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaMatHang,TenMatHang,GiaBan,GiaNhap,DangDuocBan,DangDuocHienThi,SoSao,SoLuong,SoLuongDaBan,MoTa,MaNhaCungCap,MaThuongHieu,MaDanhMuc")] MatHang matHang, int kichCo, string mauSac, IFormFile[] files)
        {
            if (id != matHang.MaMatHang)
            {
                return NotFound();
            }
            if (matHang.MoTa == null)
            {
                matHang.MoTa = "";
            }
            var mathangDB = _context.MatHangs.AsNoTracking()
                                            .Where(m => m.MaMatHang == id)
                                            .Include(m => m.MaMauSacNavigation)
                                            .Include(m => m.MaKichCoNavigation).FirstOrDefault();
            if (ModelState.IsValid && mauSac!="" && kichCo!=0)
            {
                try
                {
                    if (files != null && files.Length > 0)
                    {
                        var oldMatHangAnh = _context.MatHangAnhs.Where(m => m.MaMatHang == id).ToList();
                        foreach (var anh in oldMatHangAnh)
                        {
                            _context.Remove(anh);
                        }
                        await _context.SaveChangesAsync();

                    }

                    var hasKichCo = _context.KichCos.Where(m => m.KichCo1 == kichCo).FirstOrDefault();
                    var hasMauSac = _context.MauSacs.Where(m => m.TenMauSac == mauSac).FirstOrDefault();
                    if (hasKichCo != null)
                    {
                        matHang.MaKichCo = hasKichCo.MaKichCo;
                    }
                    else 
                    {
                        KichCo newKichCo = new KichCo();
                        newKichCo.KichCo1 = kichCo;
                        _context.KichCos.Add(newKichCo);
                        await _context.SaveChangesAsync();

                        matHang.MaKichCo = _context.KichCos.Where(m => m.KichCo1 == kichCo).FirstOrDefault().MaKichCo;
                    }
                    if (hasMauSac != null)
                    {
                        matHang.MaMauSac = hasMauSac.MaMauSac;
                    }
                    else
                    {
                        MauSac newMauSac = new MauSac();
                        newMauSac.TenMauSac = mauSac;
                        _context.MauSacs.Add(newMauSac);
                        await _context.SaveChangesAsync();
                        matHang.MaMauSac = _context.MauSacs.Where(m => m.TenMauSac == mauSac).FirstOrDefault().MaMauSac;

                    }

                    _context.MatHangs.Update(matHang);
                    await _context.SaveChangesAsync();


                    string tenMatHang = Utilities.ToTitleCase(matHang.TenMatHang);
                    if (files != null)
                    {
                        foreach (var file in files)
                        {

                            MatHangAnh matHangAnh = new MatHangAnh();
                            matHangAnh.MaMatHang = matHang.MaMatHang;
                            string extension = Path.GetExtension(file.FileName);
                            string image = Utilities.SEOUrl(tenMatHang) + DateTime.Now.Ticks + extension;
                            matHangAnh.Anh = await Utilities.UploadFile(file, @"products", image.ToLower());
                            if (string.IsNullOrEmpty(matHangAnh.Anh)) matHangAnh.Anh = "images/duphong.webp";

                            _context.MatHangAnhs.Add(matHangAnh);
                        }

                    }
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatHangExists(matHang.MaMatHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Error("Chỉnh sửa không thành công");
            ViewData["DanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc", matHang.MaDanhMuc);
            ViewData["KichCo"] = mathangDB.MaKichCoNavigation.KichCo1;
            ViewData["MauSac"] = mathangDB.MaMauSacNavigation.TenMauSac;
            ViewData["NhaCungCap"] = new SelectList(_context.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", matHang.MaNhaCungCap);
            ViewData["ThuongHieu"] = new SelectList(_context.ThuongHieus, "MaThuongHieu", "TenThuongHieu", matHang.MaThuongHieu);
            ViewData["MatHangAnh"] = new SelectList(_context.MatHangAnhs, "MaAnh", "Anh", matHang.MatHangAnhs);

            return View(matHang);
        }

        // POST: Admin/AdminMatHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var matHang = await _context.MatHangs.FindAsync(id);
            matHang.DangDuocHienThi = false;
            matHang.DangDuocBan = false;
             _context.MatHangs.Update(matHang);
            
            var theoDoi = await _context.TheoDois.Where(m => m.MaMatHang == id).ToListAsync();
            foreach(var item in theoDoi) 
                _context.TheoDois.Remove(item);

            var chiTietGioHang = await _context.ChiTietGioHangs.Where(m => m.MaMatHang == id).ToListAsync();
            foreach (var item in chiTietGioHang)
                _context.ChiTietGioHangs.Remove(item);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatHangExists(int id)
        {
            return _context.MatHangs.Any(e => e.MaMatHang == id);
        }
    }
}
