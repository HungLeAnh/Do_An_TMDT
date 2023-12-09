using Microsoft.AspNetCore.Mvc;

namespace Do_an_TMDT.Areas.User.Controllers
{
	public class CartController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
