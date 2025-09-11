using BookStore.Core.Models;
using System.Linq.Expressions;

namespace BookStore.Core.Specification
{
    public interface ISpecficationWithProjection<T, TResult> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, TResult>> Projection { get; set; }
    }
}
