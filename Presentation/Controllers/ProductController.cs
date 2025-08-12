using Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _db;

        public ProductController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index(string sortOrder)
        {
            var products = _db.Products.GetAll().ToList();

            return View(products);
        }

        public IActionResult ProductDetails(Guid id)
        {
            var product = _db.Products.GetById(id);
            return View(product);
        }
    }
}
