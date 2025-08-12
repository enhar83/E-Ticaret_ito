using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Data.Abstract
{
    public interface IUnitOfWork
    {
        IRepository<Product> Products { get; }
        IRepository<Category> Categories { get; }
        IRepository<Blog> Blogs { get; }
        IRepository<Contact> Contacts { get; }
        void Save();
    }
}
