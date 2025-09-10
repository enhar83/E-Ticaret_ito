using System.Runtime.CompilerServices;
using Data.Abstract;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _db;

        public CategoryController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            return Json(new { data = _db.Categories.GetAll().ToList() });
        }

        public IActionResult GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Json(new { success = false, message = "Geçersiz Kategori ID'si." });
            }

            var category = _db.Categories.GetAll().FirstOrDefault(p => p.Id == id);

            if (category == null)
            {
                return Json(new { success = false, message = "Kategori bulunamadı." });
            }

            var dto = new
            {
                category.Id,
                category.Title
            };
            return Json(new { success = true, data = dto });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Category category)
        {
            _db.Categories.Add(category);
            _db.Save();
            return Ok();
        }
        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Category updatedCategory)
        {
            if (updatedCategory == null || updatedCategory.Id == Guid.Empty)
                return Json(new { success = false, message = "Geçersiz kategori bilgisi." });


            var category = _db.Categories.GetFirstOrDefault(p=>p.Id==updatedCategory.Id);
            if (category == null)
                return Json(new { success = false, message = "Kategori bulunamadı." });

            category.Title = updatedCategory.Title;
            category.UpdatedDate = DateTime.Now;
            category.IsActive = updatedCategory.IsActive;

            _db.Save();
            return Json(new {success=true,message="Kategori başarıyla güncellendi."});
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            _db.Categories.Remove(id);
            _db.Save();
            return Ok();
        }

    }
}
