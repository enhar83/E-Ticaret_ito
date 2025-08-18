using Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewComponents;

namespace Presentation.Controllers
{
    public class AboutController : Controller
    {
        private readonly IUnitOfWork _db;

        public AboutController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var products = _db.Products.GetAll();
            var categories = _db.Categories.GetAll();

            ViewBag.ProductTotal = products.Count();
            ViewBag.CategoryTotal = categories.Count();

            return View();
        }
    }
}
