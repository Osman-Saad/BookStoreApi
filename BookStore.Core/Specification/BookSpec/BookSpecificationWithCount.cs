using BookStore.Core.Models;

namespace BookStore.Core.Specification.BookSpecification
{
    public class BookSpecificationWithCount : BaseSpecification<Book>
    {
        public BookSpecificationWithCount(BookSpecificationParams bookSpecParams)
            : base(
                 B => string.IsNullOrEmpty(bookSpecParams.Search) || B.Name.ToLower().Contains(bookSpecParams.Search)
                 &&
                 !bookSpecParams.CategoryId.HasValue || B.CategoryId == bookSpecParams.CategoryId
                 &&
                 !bookSpecParams.PublisherId.HasValue || B.PublisherId == bookSpecParams.PublisherId
                 &&
                 !bookSpecParams.AuthorId.HasValue || B.Authors.Any(A => A.Id == bookSpecParams.AuthorId)
            )
        {

        }
    }
}
