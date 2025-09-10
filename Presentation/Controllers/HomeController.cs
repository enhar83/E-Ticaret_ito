using System.Diagnostics;
using Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{

    public class HomeController : Controller
    {   
        private readonly IUnitOfWork _db;

        public HomeController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var products = _db.Products.GetAll(p => p.Category).ToList();
            
            return View(products);
        }

        public IActionResult HeadPartial()
        {
            return PartialView();
        }
    }
}
