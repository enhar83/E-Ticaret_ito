using Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents
{
    public class CampaignsForHome:ViewComponent
    {
        private readonly IUnitOfWork _db;

        public CampaignsForHome(IUnitOfWork db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var campaigns = _db.Campaigns.GetAll().Where(x=>x.IsActive==true).ToList();
            return View(campaigns);
        }
    }
}
