using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Abstract;
using Entity;

namespace Data.Concrete
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IRepository<Product> Products { get; private set; }
        public IRepository<Category> Categories { get; private set; }
        public IRepository<Blog> Blogs { get; private set; }
        public IRepository<Contact> Contacts { get; private set; }

        private readonly AppDbContext _db;
        public UnitOfWork( AppDbContext db)
        {
            _db = db;

            Blogs = new Repository<Blog>(_db);
            Products = new Repository<Product>(_db);
            Categories = new Repository<Category>(_db);
            Contacts = new Repository<Contact>(_db);

        }
        public int Save()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
