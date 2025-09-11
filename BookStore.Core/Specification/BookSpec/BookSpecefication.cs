using BookStore.Core.Models;

namespace BookStore.Core.Specification.BookSpecification
{
    public class BookSpecefication : BaseSpecification<Book>
    {
        public BookSpecefication(int id) : base(B => B.Id == id)
        {
            Includes.Add(B => B.Authors);
        }
    }
}
