using BockShop.DAL.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.DAL.SpecificationEvaluators
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> baseQuery, Specification<T> spec)
        {

            var query = spec.AsNoTracking ? baseQuery.AsNoTracking() : baseQuery;

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            if (spec.Includes is not null)
                foreach (var include in spec.Includes)
                    if (include is not null)
                        query = query.Include(include);
     
      
            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            if (spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);


            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;

        }
    }
}
