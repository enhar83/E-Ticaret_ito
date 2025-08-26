using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class ContactMessageViewModel
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Subject { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }

    }
}
