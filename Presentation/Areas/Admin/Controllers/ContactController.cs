using Data.Abstract;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _db;

        public ContactController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var contacts = _db.Contacts.GetAll().ToList();
            var result = contacts.Select(
                c => new
                {
                    c.Id,
                    c.Title,
                    c.Email,
                    c.Address,
                    c.PhoneNumber,
                    c.IsActive,
                    CreatedDate = c.CreatedDate.ToString("yy-MM-dd"),
                });
            return Json(new { data = result });
        }

        public IActionResult GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Json(new { success = false, message = "Geçersiz İletişim Bilgisi ID'si." });
            }

            var contact = _db.Contacts.GetAll().FirstOrDefault(p => p.Id == id);

            if (contact == null)
            {
                return Json(new { success = false, message = "İletişim bilgisi bulunamadı." });
            }

            var dto = new
            {
                contact.Id,
                contact.Title,
                contact.PhoneNumber,
                contact.Email,
                contact.Address,
                contact.IsActive
            };

            return Json(new { success = true, data = dto });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Contact contact)
        {
            contact.CreatedDate = DateTime.Now;
            contact.UpdatedDate = DateTime.Now;

            _db.Contacts.Add(contact);
            _db.Save();
            return Ok();
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Contact updatedContact)
        {
            if (updatedContact == null || updatedContact.Id == Guid.Empty)
                return Json(new { success = false, message = "Geçersiz iletişim bilgisi." });


            var contact = _db.Contacts.GetFirstOrDefault(p => p.Id == updatedContact.Id);
            if (contact == null)
                return Json(new { success = false, message = "İletişim bilgisi bulunamadı." });

            contact.Title = updatedContact.Title;
            contact.Email = updatedContact.Email;
            contact.Address = updatedContact.Address;
            contact.PhoneNumber = updatedContact.PhoneNumber;
            contact.IsActive = updatedContact.IsActive;
            contact.UpdatedDate = DateTime.Now;


            _db.Contacts.Update(contact);
            _db.Save();
            return Json(new { success = true, message = "İletişim bilgisi başarıyla güncellendi." });
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            _db.Contacts.Remove(id);
            _db.Save();
            return Ok();
        }

    }
}
