using Microsoft.AspNetCore.Mvc;

namespace Wolmart_Ecommerce_Marketplace.Controllers
{
    public class SellerSetupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult RegisterAsSeller()
        {
            return View();
        }
    }
}
