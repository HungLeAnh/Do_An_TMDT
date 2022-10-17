using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_an_TMDT.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        [Route("admin.html", Name = "AdminIndex")]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
