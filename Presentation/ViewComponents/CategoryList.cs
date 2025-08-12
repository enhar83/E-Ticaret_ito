using Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents
{
    public class CategoryList:ViewComponent
    {
        private readonly IUnitOfWork _db;

        public CategoryList(IUnitOfWork db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            return View(_db.Categories.GetAll().ToList());
        }
    }
}
