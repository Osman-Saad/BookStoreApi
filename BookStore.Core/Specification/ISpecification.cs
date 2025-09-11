using BookStore.Core.Models;
using System.Linq.Expressions;

namespace BookStore.Core.Specification
{
    public interface ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public Expression<Func<T, object>>? OrderByAsc { get; set; }
        public Expression<Func<T, object>>? OrderByDesc { get; set; }
        public bool PaginationIsEnable { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}
