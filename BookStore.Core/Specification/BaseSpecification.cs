using BookStore.Core.Models;
using System.Linq.Expressions;

namespace BookStore.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>>? OrderByAsc { get; set; } = null;
        public Expression<Func<T, object>>? OrderByDesc { get; set; } = null;
        public bool PaginationIsEnable { get; set; }

        public int Take { get; set; }
        public int Skip { get; set; }
        public BaseSpecification()
        {

        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public void SetOrderByAsc(Expression<Func<T, object>> orderByAsc) =>
            OrderByAsc = orderByAsc;
        public void SetOrderByDesc(Expression<Func<T, object>> orderByDesc) =>
            OrderByDesc = orderByDesc;

        public void ApplyPagination(int take, int skip)
        {
            PaginationIsEnable = true;
            Take = take;
            Skip = skip;
        }
    }
}
