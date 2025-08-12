using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Abstract
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, Object>>[]includeProperties);

        IQueryable<T> GetAllDelete(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAllDelete();

        void Add(T entity);
        void Update(T entity);
        void Remove(Guid id);
        T GetFirstOrDefault(Expression<Func<T,bool>>predicate);
        T GetById(Guid id);
    }
}
