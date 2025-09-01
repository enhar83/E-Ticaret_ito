using AspNetCoreGeneratedDocument;
using Data.Abstract;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Logging;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.JSInterop.Implementation;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _db;

        public BlogController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            return Json(new { data = _db.Blogs.GetAll().ToList() });
        }

        public IActionResult GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Json(new { success = false, message = "Geçersiz Blog ID'si." });
            }

            var blog = _db.Blogs.GetAll().FirstOrDefault(x => x.Id == id);

            if (blog == null)
            {
                return Json(new { success = false, message = "Blog Bulunamadı." });
            }

            var dto = new
            {
                blog.Id,
                blog.Title,
                blog.Description,
                blog.ImageUrl
            };

            return Json(new { success = true, data = dto });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Blog blog,IFormFile imageFile)
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

                blog.ImageUrl = fileName;
            }

            blog.CreatedDate = DateTime.Now;
            blog.UpdatedDate = DateTime.Now;

            _db.Blogs.Add(blog);
            _db.Save();
            return Ok();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Edit(Blog updatedBlog,IFormFile imageFile)
        {
            if (updatedBlog == null || updatedBlog.Id == Guid.Empty)
                return Json(new { success = false, message = "Geçersiz blog bilgisi" });

            var blog = _db.Blogs.GetFirstOrDefault(x => x.Id == updatedBlog.Id);

            if (blog == null)
                return Json(new { success = false, message = "Blog bulunamadı." });

            blog.Title= updatedBlog.Title;
            blog.Description = updatedBlog.Description;
            blog.IsActive = updatedBlog.IsActive;
            blog.UpdatedDate = DateTime.Now;

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(blog.ImageUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", blog.ImageUrl);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                    imageFile.CopyTo(stream);

                blog.ImageUrl = fileName;
            }

            _db.Save();
            return Ok();
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            _db.Blogs.Remove(id);
            _db.Save();
            return Ok();
        }

    }
}

