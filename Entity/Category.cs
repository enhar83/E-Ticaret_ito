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
        public int? ParentCategoryId { get; set; }
        public virtual Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new HashSet<Category>();
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
