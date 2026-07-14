using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BockShop.DAL.Specifications
{
    public class Specification<T>
    {
        public bool AsNoTracking { get; protected set; }
        public Expression<Func<T, bool>>? Criteria { get; protected set; }

        public List<Expression<Func<T, object>>?>? Includes { get; }
            = new();

        public Expression<Func<T, object>>? OrderBy { get; protected set; }

        public Expression<Func<T, object>>? OrderByDescending { get; protected set; }

        public int Skip { get; protected set; }

        public int Take { get; protected set; }

        public bool IsPagingEnabled { get; protected set; }

        protected void AddInclude(Expression<Func<T, object>> include)
        {
            Includes.Add(include);
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDesc)
        {
            OrderByDescending = orderByDesc;
        }
    }
}
