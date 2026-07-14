using BockShop.DAL.Context;
using BockShop.DAL.Interfaces;
using BockShop.DAL.SpecificationEvaluators;
using BockShop.DAL.Specifications;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace BockShop.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _dbSet = _applicationDbContext.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }
        public async Task<T?> GetFirstOneAsync(Expression<Func<T, bool>>? criteria)
        {
            if (criteria == null)
                return await _dbSet.FirstOrDefaultAsync();

            return await _dbSet.FirstOrDefaultAsync(criteria);
        }
        public IQueryable<T> GetAll(Expression<Func<T, bool>>? criteria = null, Expression<Func<T, bool>>? search = null,
        bool tracking = false
      , Expression<Func<T, object>>? orderBy = null, bool descending = false, params Expression<Func<T, object>>?[] includes)
        {
            var entities = _dbSet.AsQueryable();
            if (!tracking)
                entities = entities.AsNoTracking();
            if (search is not null)
                entities = entities.Where(search);
            if (criteria is not null)
                entities = entities.Where(criteria);
            if (orderBy is not null)
            {
                entities = descending ? entities.OrderByDescending(orderBy) : entities.OrderBy(orderBy);
            }

            if (includes is not null)
                foreach (var include in includes)
                    if (include is not null)
                        entities = entities.Include(include);

            return entities;
        }
        public async Task<IEnumerable<T>> GetAll(Specification<T> specification)
        {
            return await SpecificationEvaluator<T>.GetQuery(_applicationDbContext.Set<T>(), specification).ToListAsync();
        }
        public async Task<T?> GetOne(Specification<T> specification)
        {
            return await SpecificationEvaluator<T>.GetQuery(_applicationDbContext.Set<T>(), specification).FirstOrDefaultAsync();
        }



    }
}
