using Data.Abstract;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _db;

        public ProductController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var products = _db.Products.GetAll().ToList();
            ViewBag.totalCount = products.Count;

            return View(products);
        }

        public IActionResult ProductDetails(Guid id)
        {
            var product = _db.Products.GetById(id);
            return View(product);
        }

        public IActionResult Compare(Guid id, Guid? comparedProductId)
        {
            var selectedProduct = _db.Products.GetAll(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (selectedProduct == null)
            {
                return NotFound();
            }

            var selectedCategoryProducts = _db.Products.GetAll(p => p.Category)
                .Where(p => p.CategoryId == selectedProduct.CategoryId && p.Id != selectedProduct.Id)
                .ToList();

            Product? comparedProduct = null;
            if (comparedProductId.HasValue)
            {
                comparedProduct = selectedCategoryProducts.FirstOrDefault(p => p.Id == comparedProductId.Value);
            }

            var compareViewModel = new CompareViewModel
            {
                SelectedProduct = selectedProduct,
                SelectedCategoryProducts = selectedCategoryProducts,
                ComparedProduct = comparedProduct
            };

            return View(compareViewModel);
        }

        public IActionResult FilterPrice(decimal minPrice, decimal maxPrice)
        {
            var filteredProducts = _db.Products.GetAll()
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
              .OrderByDescending(p => p.Price)  
                .ToList();

            ViewBag.FilteredCount = filteredProducts.Count;
            return View("Index", filteredProducts);
        }
    }
}
