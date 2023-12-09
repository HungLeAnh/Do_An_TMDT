using Do_an_TMDT.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Do_an_TMDT.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Linq;
using Do_an_TMDT.Helpper;
using Microsoft.EntityFrameworkCore;
using Do_an_TMDT.ViewModels;
using Do_an_TMDT.Extension;

namespace Do_an_TMDT.Areas.Shipper.Controllers
{
    [Area("Shipper")]

    [AllowAnonymous]
    public class ShipperLoginController : Controller
    {
        private readonly ILogger<ShipperLoginController> _logger;
        private readonly WEBBANGIAYContext _context;
        public INotyfService _notyfService { get; }
        public ShipperLoginController(WEBBANGIAYContext context, ILogger<ShipperLoginController> logger, INotyfService notyfService)
        {
            _context = context;
            _logger = logger;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("Shipper/login", Name ="ShipperLogin",Order = 1)]
        public IActionResult Login(string ReturnUrl = "")
        {

            LoginViewModel objLoginModel = new LoginViewModel();
            objLoginModel.ReturnUrl = ReturnUrl;

            return View(objLoginModel);
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Shipper/login", Name = "ShipperLogin",Order =1)]
        public async Task<IActionResult> Login(LoginViewModel objLoginModel)
        {
            if (ModelState.IsValid)
            {
                NguoiDung userInDatabase = new NguoiDung();

                if (Utilities.IsValidEmail(objLoginModel.UserName))
                {
                    userInDatabase = await _context.NguoiDungs.Where(x => x.Email == objLoginModel.UserName)
                                                                .Include(x => x.MaLoaiNguoiDungNavigation)
                                                                .FirstOrDefaultAsync();
                }
                else
                {
                    userInDatabase = await _context.NguoiDungs.Where(x => x.TenDangNhap == objLoginModel.UserName)
                                                              .Include(x => x.MaLoaiNguoiDungNavigation)
                                                              .FirstOrDefaultAsync();
                }
                if (userInDatabase != null)
                {
                    string hashPassword = (objLoginModel.Password + userInDatabase.Salt.Trim()).ToMD5();

                    if (hashPassword == userInDatabase.MatKhauHash &&
                        (userInDatabase.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.Equals("Shipper", System.StringComparison.OrdinalIgnoreCase) ||
                        userInDatabase.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.Equals("Người Giao Hàng", System.StringComparison.OrdinalIgnoreCase) ||
                        userInDatabase.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.Equals("Nhân viên", System.StringComparison.OrdinalIgnoreCase)))
                    {
                        //A claim is a statement about a subject by an issuer and    
                        //represent attributes of the subject that are useful in the context of authentication and authorization operations.
                        var claims = new List<Claim>() {
                            new Claim(ClaimTypes.Sid, userInDatabase.MaNguoiDung.ToString()),
                            new Claim(ClaimTypes.Name, userInDatabase.TenNguoiDung),
                            new Claim(ClaimTypes.Role, userInDatabase.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung),
                        };
                        //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                        var principal = new ClaimsPrincipal(identity);
                        //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                        await HttpContext.SignInAsync("ShipperLogin", principal, new AuthenticationProperties()
                        {
                            IsPersistent = false,
                        });
                         return RedirectToAction("", "Home", new { area = "Shipper" });
                    }
                    else
                    {
                        ViewBag.Message = "Đăng nhập không thành công";
                        return View(objLoginModel);

                    }
                }
                else
                {
                    ViewBag.Message = "Invalid Credential";
                    return View(objLoginModel);
                }
            }
            return View(objLoginModel);
        }
        [Route("ShipperLogout",Name ="ShipperLogout")]
        public async Task<IActionResult> Logout()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync("ShipperLogin");
            //Redirect to home page    
            return LocalRedirect("/");
        }
    }

}
