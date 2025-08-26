using System.Net.Mail;
using Data.Abstract;
using Entity;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Presentation.Models;
using MailKit.Net.Smtp;



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
            var contact = _db.Contacts.GetFirstOrDefault(x => true);
            return View(contact);
        }

        [HttpGet]
        public IActionResult Message()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Message(ContactMessageViewModel contact)
        {
            if (ModelState.IsValid)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("İletişim Formu", "enharyrlmzstaj@gmail.com"));
                message.To.Add(new MailboxAddress("Alıcı", "enharyrlmz@gmail.com"));

                message.Subject = contact.Subject;
                message.Body = new TextPart("plain")
                {
                    Text = $"Gönderen: {contact.Name}\nE-posta: {contact.Mail}\nTelefon Numarası: {contact.PhoneNumber}\nMesaj:\n{contact.Message}"
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {

                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate("enharyrlmzstaj@gmail.com", "bqkp srhj kroe hnpc");
                    client.Send(message);
                    client.Disconnect(true);
                }
                return Json(new {success=true,message="Form başarıyla gönderildi."});
            }
            return Json(new { success = false, message = "Formda eksik alanlar var." });
        }
    }
}
