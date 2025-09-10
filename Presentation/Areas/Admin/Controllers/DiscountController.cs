using Data.Abstract;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class DiscountController : Controller
    {
        private readonly IUnitOfWork _db;

        public DiscountController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var discounts = _db.Discounts.GetAll(d => d.Category).ToList();
            var result = discounts.Select(d => new
            {
                d.Id,
                d.Title,
                d.DiscountAmount,
                d.DiscountRate,
                d.IsActive,
                Category = d.Category.Title,
                CreatedDate = d.CreatedDate
            });

            return Json(new { data = result });
        }

        public IActionResult GetById(Guid id)
        {
            if (id == Guid.Empty)
                return Json(new { success = false, message = "Geçersiz indirim ID'si." });

            var discount = _db.Discounts.GetAll(d => d.Category).FirstOrDefault(d => d.Id == id);
            if (discount == null)
                return Json(new { success = false, message = "İndirim bulunamadı." });

            var dto = new
            {
                discount.Id,
                discount.Title,
                discount.DiscountAmount,
                discount.DiscountRate,
                CategoryId = discount.CategoryId,
                discount.IsActive
            };

            return Json(new { success = true, data = dto });
        }
        public IActionResult GetAllCategories()
        {
            var categories = _db.Categories.GetAll()
                .Select(c => new { c.Id, c.Title })
                .ToList();

            return Json(categories);
        }

        [HttpPost]
        public IActionResult Add(Discount discount)
        {
            if (discount.DiscountRate > 100 || discount.DiscountRate < 0)
                return Json(new { success = false, message = "İndirim oranı 0 ile 100 arasında olmalıdır." });

            _db.Discounts.Add(discount);
            _db.Save();
            return Ok();
        }

        [HttpPost]
        public IActionResult Edit(Discount updatedDiscount)
        {
            if (updatedDiscount == null || updatedDiscount.Id == Guid.Empty)
                return Json(new { success = false, message = "Geçersiz indirim ID'si." });

            var discount = _db.Discounts.GetFirstOrDefault(d => d.Id == updatedDiscount.Id);
            if (discount == null)
                return Json(new { success = false, message = "İndirim bulunamadı." });

            if (updatedDiscount.DiscountRate > 100 || updatedDiscount.DiscountRate < 0)
                return Json(new { success = false, message = "İndirim oranı 0 ile 100 arasında olmalıdır." });

            discount.Title = updatedDiscount.Title;
            discount.DiscountAmount = updatedDiscount.DiscountAmount;
            discount.DiscountRate = updatedDiscount.DiscountRate;
            discount.CategoryId = updatedDiscount.CategoryId;
            discount.IsActive = updatedDiscount.IsActive;
            discount.UpdatedDate = DateTime.Now;

            _db.Save();
            return Json(new { success = true, message = "İndirim başarıyla güncellendi." });
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            _db.Discounts.Remove(id);
            _db.Save();
            return Ok();
        }

    }
}
