using Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class CampaignController : Controller
    {
        private readonly IUnitOfWork _db;

        public CampaignController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var campaigns = _db.Campaigns.GetAll().ToList();
            return View(campaigns);
        }

        public IActionResult CampaignDetails(Guid id)
        {
            var campaign = _db.Campaigns.GetById(id);
            return View(campaign);
        }
    }
}
