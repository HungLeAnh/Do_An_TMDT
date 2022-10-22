using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Do_an_TMDT.Areas.Admin.Controllers
{
    [Area("admin")]
    public class HomeController : Controller
    {
        public INotyfService _notyfService { get; }
        // GET: /<controller>/

        public IActionResult Index()
        {
            return View();
        }
    }
}
