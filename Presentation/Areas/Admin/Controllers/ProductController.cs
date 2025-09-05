using Data.Abstract;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _db;

        public ProductController(IUnitOfWork db)
        {
            _db = db;
        }


        private static readonly string[] AllowedExt = { ".jpg", ".jpeg", ".png", ".webp", ".avif" };
        private static readonly string[] AllowedMime = { "image/jpeg", "image/png", "image/webp", "image/avif" };
        private const long MaxUploadBytes = 10 * 1024 * 1024;

        private string ValidateImageFiles(params IFormFile[] files)
        {
            foreach (var f in files)
            {
                if (f == null || f.Length == 0) continue;

                var ext = Path.GetExtension(f.FileName).ToLowerInvariant();
                var mime = (f.ContentType ?? "").ToLowerInvariant();

                if (f.Length > MaxUploadBytes)
                    return "Dosya boyutu 10MB'ı aşamaz.";

                if (!AllowedExt.Contains(ext) || !AllowedMime.Contains(mime))
                    return "Sadece JPG, PNG veya WebP (opsiyonel AVIF) yükleyebilirsiniz.";
            }
            return null;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var products = _db.Products.GetAll(p => p.Category).ToList();
            var result = products.Select(
                p => new
                {
                    p.Id,
                    p.Title,
                    p.Price,
                    p.Stock,
                    p.Description,
                    p.ImageUrl,
                    p.ProductCode,
                    Category = p.Category.Title,
                    p.IsActive,
                    CreatedDate = p.CreatedDate.ToString("yy-MM-dd"),
                });
            return Json(new {data=result});
        }

        public IActionResult GetById(Guid id)
        {
            if (id == Guid.Empty)
                return Json(new { success = false, message = "Geçersiz ürün ID'si." });

            var product = _db.Products
                             .GetAll(p => p.Category)
                             .FirstOrDefault(p => p.Id == id);
            if (product == null)
                return Json(new { success = false, message = "Ürün bulunamadı." });

            var dto = new
            {
                product.Id,
                product.Title,
                product.Price,
                product.Stock,
                product.Description,
                product.ProductCode, // Kod gösterimi
                CategoryId = product.CategoryId,
                product.ImageUrl,
                product.IsActive
            };

            return Json(new { success = true, data = dto });
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _db.Categories.GetAll()
                .Select(c => new { c.Id, c.Title })
                .ToList();

            return Json(categories);
        }


        [HttpGet]
        public IActionResult Add()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product product,IFormFile imageFile)
        {
            // Resim yükleme işlemi
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                product.ImageUrl = fileName;
            }

            product.CreatedDate = DateTime.Now;
            product.UpdatedDate = DateTime.Now;

            _db.Products.Add(product);
            _db.Save();

            return Ok();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Product updatedProduct,IFormFile imageFile)
        {
            if (updatedProduct == null || updatedProduct.Id == Guid.Empty)
                return Json(new { success = false, message = "Geçersiz ürün bilgisi." });

            var product = _db.Products.GetFirstOrDefault(p => p.Id == updatedProduct.Id);
            if (product == null)
                return Json(new { success = false, message = "Ürün bulunamadı." });

            product.Title = updatedProduct.Title;
            product.Price = updatedProduct.Price;
            product.Stock = updatedProduct.Stock;
            product.Description = updatedProduct.Description;
            product.CategoryId = updatedProduct.CategoryId;
            product.IsActive = updatedProduct.IsActive;
            product.UpdatedDate = DateTime.Now;

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", product.ImageUrl);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                    imageFile.CopyTo(stream);

                product.ImageUrl = fileName;
            }

            _db.Save();

            return Json(new { success = true, message = "Ürün başarıyla güncellendi." });
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            _db.Products.Remove(id);
            _db.Save();
            return Ok();
        }
    }
}
