using Data.Abstract;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class CampaignController : Controller
    {
        private readonly IUnitOfWork _db;

        public CampaignController(IUnitOfWork db)
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
            return Json(new { data = _db.Campaigns.GetAll().ToList() });
        }

        public IActionResult GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Json(new { success = false, message = "Geçersiz Kampanya ID'si." });
            }
            var campaign = _db.Campaigns.GetAll().FirstOrDefault(p => p.Id == id);

            if (campaign == null)
            {
                return Json(new { success = false, message = "Kampanya bulunamadı." });
            }

            var dto = new
            {
                campaign.Id,
                campaign.Title,
                campaign.Description,
                campaign.ImageUrl,
                campaign.IsActive
            };
            return Json(new { success = true, data = dto });
        }

        [HttpPost]
        public IActionResult Add(Campaign campaign, IFormFile imageFile)
        {
            var err = ValidateImageFiles(imageFile);
            if (err != null) return BadRequest(err);

            var imagesRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(imagesRoot);

            // Tek bir yardımcı metod ile kaydet
            string SaveAndGetFileName(IFormFile file)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var uploadPath = Path.Combine(imagesRoot, fileName);
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return fileName;
            }

            // imageFile -> ImageUrl
            if (imageFile != null && imageFile.Length > 0)
                campaign.ImageUrl = SaveAndGetFileName(imageFile);


            _db.Campaigns.Add(campaign);
            _db.Save();
            return Ok();
        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Campaign updatedCampaign,IFormFile imageFile)
        {
            if (updatedCampaign == null || updatedCampaign.Id == Guid.Empty)
                return Json(new { success = false, message = "Geçersiz kampanya bilgisi." });

            var campaign = _db.Campaigns.GetFirstOrDefault(p => p.Id == updatedCampaign.Id);
            if (campaign == null)
                return Json(new { success = false, message = "Kampanya bulunamadı." });

            var err = ValidateImageFiles(imageFile);
            if (err != null) return Json(new { success = false, message = err });

            campaign.Title = updatedCampaign.Title;
            campaign.Description = updatedCampaign.Description;
            campaign.UpdatedDate= DateTime.Now;
            campaign.IsActive = updatedCampaign.IsActive;

            var imagesRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(imagesRoot);

            // Tek noktadan dosya kaydet/sırala (yeni yoksa eskisini korur)
            string SaveOrKeep(IFormFile file, string oldName)
            {
                if (file == null || file.Length == 0) return oldName;

                // Eski dosyayı sil
                if (!string.IsNullOrWhiteSpace(oldName))
                {
                    var oldPath = Path.Combine(imagesRoot, oldName);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // Yeni dosyayı kaydet
                var newName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                using (var fs = new FileStream(Path.Combine(imagesRoot, newName), FileMode.Create))
                {
                    file.CopyTo(fs);
                }
                return newName;
            }

            // 1–3 görseli işle (yalnızca gönderilenleri değiştirir)
            campaign.ImageUrl = SaveOrKeep(imageFile, campaign.ImageUrl);

            _db.Save();
            return Json(new { success = true, message = "Kampanya başarıyla güncellendi." });
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            _db.Campaigns.Remove(id);
            _db.Save();
            return Ok();
        }
    }

}
