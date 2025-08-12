using Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents
{
    public class SimilarProducts:ViewComponent
    {
        private readonly IUnitOfWork _db;
        public SimilarProducts(IUnitOfWork db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke(Guid productId)
        {
            var product = _db.Products.GetById(productId);
            
            var similarProducts = _db.Products.GetAll(p => p.Category)
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .ToList();
            return View(similarProducts);
        }
    }
}
