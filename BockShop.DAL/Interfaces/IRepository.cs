using BockShop.DAL.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BockShop.DAL.Interfaces
{
    public  interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAll();
        Task<IEnumerable<T>> GetAll(Specification<T> specification);
        Task<T?> GetFirstOneAsync(Expression<Func<T, bool>>? criteria);
         Task<T?> GetOne(Specification<T> specification);
        IQueryable<T> GetAll(Expression<Func<T, bool>>? criteria = null, Expression<Func<T, bool>>? search = null,bool tracking=false
      , Expression<Func<T, object>>? orderBy = null, bool descending = false, params Expression<Func<T, object>>?[] includes);
    }
}
