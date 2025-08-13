using Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents
{
    public class LatestProducts : ViewComponent
    {
        private readonly IUnitOfWork _db;
        public LatestProducts(IUnitOfWork db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var products = _db.Products.GetAll(p => p.Category).OrderByDescending(p => p.CreatedDate).Take(3).ToList();
            return View(products);
        }
    }
}
