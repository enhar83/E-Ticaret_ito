using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Entity
{
    public class Category:BaseModel
    {
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
        public ICollection<Discount> Discounts { get; set; } = new HashSet<Discount>();
    }
}
