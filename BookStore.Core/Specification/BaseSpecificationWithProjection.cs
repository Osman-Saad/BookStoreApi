using BookStore.Core.Models;
using System.Linq.Expressions;

namespace BookStore.Core.Specification
{
    public class BaseSpecificationWithProjection<T, TResult> : BaseSpecification<T>, ISpecficationWithProjection<T, TResult> where T : BaseEntity
    {
        public Expression<Func<T, TResult>> Projection { get; set; } = null;

        public BaseSpecificationWithProjection(Expression<Func<T, bool>> criteria) : base(criteria)
        {
        }
        public BaseSpecificationWithProjection()
        {

        }
    }
}
