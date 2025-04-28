using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            //apply filtering : WHERE clause
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            //apply sorting: ASC
            if (spec.OrderByAscend != null)
            {
                query = query.OrderBy(spec.OrderByAscend);
            }

            //apply sorting: DESC
            if (spec.OrderByDescend != null)
            {
                query = query.OrderByDescending(spec.OrderByDescend);
            }

            
            // Apply eager loading for navigation properties
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));


            //apply pagination
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;
        }
    }
}
