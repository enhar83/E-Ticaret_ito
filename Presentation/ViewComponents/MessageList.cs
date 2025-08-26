using Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.ViewComponents
{
    public class MessageList : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var message = new ContactMessageViewModel();
            return View(message);
        }

    }
}
