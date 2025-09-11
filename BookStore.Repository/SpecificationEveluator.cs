using BookStore.Core.Models;
using BookStore.Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class SpecificationEveluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> table, ISpecification<T> spec)
        {
            var query = table;
            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);
            if (spec.OrderByAsc != null)
                query = query.OrderBy(spec.OrderByAsc);
            if (spec.OrderByDesc != null)
                query = query.OrderByDescending(spec.OrderByDesc);
            if (spec.PaginationIsEnable)
                query = query.Skip(spec.Skip).Take(spec.Take);
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;

        }

        public static IQueryable<TResult> ApplySpecificationWithProjection<TResult>(
        IQueryable<T> table,
        ISpecficationWithProjection<T, TResult> spec
        )
        {
            var query = GetQuery(table, spec);
            return query.Select(spec.Projection);
        }
    }
}
