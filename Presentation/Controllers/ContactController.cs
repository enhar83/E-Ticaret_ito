using Data.Abstract;
using Entity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _db;

        public ContactController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var contact = _db.Contacts.GetFirstOrDefault(x=>true);
            return View();
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Contact contact)
        {
            _db.Contacts.Add(contact);
            _db.Save();

            return Ok();
        }
    }
}
