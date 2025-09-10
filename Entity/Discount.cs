using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Discount:BaseModel
    {
        public decimal? DiscountRate { get; set; }
        public decimal? DiscountAmount { get; set; }

        public Guid CategoryId { get; set; }
        public required Category Category { get; set; }
    }
}
