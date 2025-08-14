using Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using NuGet.Protocol.Plugins;
using Presentation.Helpers;
using Presentation.Models.CartViewModels;

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
            var cart = HttpContext.Session.GetObject<CartViewModel>("Cart"); //"Cart" helperstaki key oluyor
            if (cart == null)
                cart = new CartViewModel();

            return View(cart);
        }

        public IActionResult AddToCart(Guid productId, int quantity = 1)
        {
            var product = _db.Products.GetFirstOrDefault(x => x.Id == productId);
            if (product == null)
                return Json(new { success = false, message = "Ürün bulunamadı." });

            var cart = HttpContext.Session.GetObject<CartViewModel>("Cart"); //deserialize
            if (cart == null)
                cart = new CartViewModel();

            var existingProduct = cart.Items.FirstOrDefault(c => c.ProductId == productId);
            if (existingProduct != null)
            {
                existingProduct.Quantity += quantity;
                HttpContext.Session.SetObject("Cart", cart);
                return Json(new
                {
                    success = true,
                    message = $"{product.Title} miktarı güncellendi.",
                    cartItemCount = cart.Items.Sum(x => x.Quantity),
                    cartTotal = cart.Items.Sum(x => x.TotalPrice)
                });
            }

            else
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }

            HttpContext.Session.SetObject("Cart", cart); //cartviewmodeli jsona çevirip sessiona koyar.(serialize)

            Console.WriteLine($"Eklendi: {product.Title} - {product.Id}");

            return Json(new
            {
                success = true,
                message = $"{product.Title} sepete eklendi.",
                cartItemCount = cart.Items.Sum(x => x.Quantity),
                cartTotal = cart.Items.Sum(x => x.TotalPrice),
            });
        }


        [HttpPost]
        public IActionResult UpdateQuantity(Guid productId, int quantity)
        {
            var cart = HttpContext.Session.GetObject<CartViewModel>("Cart");
            if (cart == null)
                return Json(new { success = false, message = "Sepet boş." });

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                    cart.Items.Remove(item);
                else
                    item.Quantity = quantity;
            }

            HttpContext.Session.SetObject("Cart", cart);
            return Json(new
            {
                success = true,
                message = $"{item?.Title ?? "Ürün"} miktarı güncellendi.",
                cartItemCount = cart.Items.Sum(x => x.Quantity),
                cartTotal = cart.Items.Sum(x => x.TotalPrice),
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(Guid productId)
        {
            var cart = HttpContext.Session.GetObject<CartViewModel>("Cart");
            if (cart == null)
                return Json(new { success = false, message = "Sepet boş." });

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                cart.Items.Remove(item);
            }

            HttpContext.Session.SetObject("Cart", cart);
            return Json(new
            {
                success = true,
                message = $"{item?.Title ?? "Ürün"} sepetten kaldırıldı.",
                cartItemCount = cart.Items.Sum(x => x.Quantity),
                cartTotal = cart.Items.Sum(x => x.TotalPrice),
            });

        }

        public IActionResult GetCart()
        {
            // Session’dan CartViewModel’i al
            var cart = HttpContext.Session.GetObject<CartViewModel>("Cart");

            if (cart == null || cart.Items.Count == 0)
            {
                return Json(new
                {
                    items = new List<object>(),
                    total = 0,
                    cartItemCount = 0
                });
            }

            // Minicart için gerekli bilgileri maple
            var items = cart.Items.Select(x => new
            {
                productId = x.ProductId,
                title = x.Title,
                price = x.Price,
                quantity = x.Quantity,
                totalPrice = x.TotalPrice,
                imageUrl = x.ImageUrl
            }).ToList();

            var total = cart.Items.Sum(x => x.TotalPrice);
            var cartItemCount = cart.Items.Sum(x => x.Quantity);

            return Json(new
            {
                items = items,
                total = total,
                cartItemCount = cartItemCount
            });
        }
    }
}
