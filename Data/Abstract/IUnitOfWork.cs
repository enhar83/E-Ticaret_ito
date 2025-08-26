using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Data.Abstract
{
    public interface IUnitOfWork : IDisposable //disposable eklenmesi, kaynakların düzgün bir şekilde serbest bırakılmasını sağlar.
    {
        IRepository<Product> Products { get; }
        IRepository<Category> Categories { get; }
        IRepository<Blog> Blogs { get; }
        IRepository<Contact> Contacts { get; }
        int Save(); //int olarak olması daha iyi çünkü SaveChanges() metodu int döner ve bu değişikliklerin sayısını verir.
    }
}
