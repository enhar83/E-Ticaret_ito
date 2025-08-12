using Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents
{
    public class SpecificationByCategory:ViewComponent
    {
        private readonly IUnitOfWork _db;

        public SpecificationByCategory(IUnitOfWork db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke(Guid productId)
        {
            var product = _db.Products.GetById(productId);
            var category = _db.Categories.GetById(product.CategoryId);

            return View(category);
        }
    }
}
