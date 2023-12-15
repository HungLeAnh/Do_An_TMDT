using Do_an_CCNPMM.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Do_an_CCNPMM.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Linq;
using Do_an_CCNPMM.Helpper;
using Microsoft.EntityFrameworkCore;
using Do_an_CCNPMM.ViewModels;
using Do_an_CCNPMM.Extension;

namespace Do_an_CCNPMM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class AdminLoginController : Controller
    {

        private readonly ILogger<AdminLoginController> _logger;
        private readonly WEBBANGIAYContext _context;
        public INotyfService _notyfService { get; }

        public AdminLoginController(WEBBANGIAYContext context,ILogger<AdminLoginController> logger, INotyfService notyfService)
        {
            _context = context;
            _logger = logger;
            _notyfService = notyfService;
        }

        [AllowAnonymous]
        [Route("Admin/login", Name = "AdminLogin")]
        public IActionResult Login(string ReturnUrl = "")
        {

            LoginViewModel objLoginModel = new LoginViewModel();
            objLoginModel.ReturnUrl = ReturnUrl;

            return View(objLoginModel);
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Admin/login", Name = "AdminLogin")]
        public async Task<IActionResult> Login(LoginViewModel objLoginModel)
        {
            if (ModelState.IsValid)
            {
                NguoiDung userInDatabase = new NguoiDung();

                if (Utilities.IsValidEmail(objLoginModel.UserName))
                {
                    userInDatabase = await _context.NguoiDungs.Where(x => x.Email == objLoginModel.UserName)
                                                                .Include(x=>x.MaLoaiNguoiDungNavigation)
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
                        (userInDatabase.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.Equals("Admin",System.StringComparison.OrdinalIgnoreCase)||
                        userInDatabase.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.Equals("Quản lí", System.StringComparison.OrdinalIgnoreCase)||
                        userInDatabase.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung.Equals("Quản lý", System.StringComparison.OrdinalIgnoreCase) ))
                    {
                        //A claim is a statement about a subject by an issuer and    
                        //represent attributes of the subject that are useful in the context of authentication and authorization operations.
                        var claims = new List<Claim>() {
                            new Claim(ClaimTypes.Name, userInDatabase.TenNguoiDung),
                            new Claim(ClaimTypes.Role, userInDatabase.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung),
                        };
                        //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                        var principal = new ClaimsPrincipal(identity);
                        //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                        await HttpContext.SignInAsync("AdminLogin", principal, new AuthenticationProperties()
                        {
                            IsPersistent = false,
                        });
                        return LocalRedirect(objLoginModel.ReturnUrl);
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
        public async Task<IActionResult> Logout()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync("AdminLogin");
            //Redirect to home page    
            return LocalRedirect("/");
        }

    }
}
