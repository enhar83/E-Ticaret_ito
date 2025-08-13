using Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _db;

        public CartController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartViewModel>>("Cart") ?? new List<CartViewModel>();
            return View(cart);
        }

        public IActionResult AddToCart(Guid productId,int quantity=1)
        {
            var product=_db.Products.GetFirstOrDefault(x=> x.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            List<CartViewModel> cart = HttpContext.Session.GetObjectFromJson<List<CartViewModel>>("Cart") ?? new List<CartViewModel>();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartViewModel
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Remove(Guid productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartViewModel>>("Cart") ?? new List<CartViewModel>();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(Guid productId, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartViewModel>>("Cart") ?? new List<CartViewModel>();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
    }
}
