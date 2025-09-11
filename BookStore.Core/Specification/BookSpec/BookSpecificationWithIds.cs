using BookStore.Core.Models;

namespace BookStore.Core.Specification.BookSpec
{
    public class BookSpecificationWithIds : BaseSpecification<Book>
    {
        public BookSpecificationWithIds(List<int> ids) : base(B => ids.Contains(B.Id))
        {

        }
    }
}
