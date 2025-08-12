using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Category:BaseModel
    {
        public ICollection<Product> Products { get; set; }
    }
}
