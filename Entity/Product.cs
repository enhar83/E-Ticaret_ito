using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Product:BaseModel
    {
        public required string ImageUrl { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public required string Description { get; set; }
        public string? ProductCode { get; set; }
        public Guid CategoryId { get; set; }
        public required virtual Category Category { get; set; }
    }
}
