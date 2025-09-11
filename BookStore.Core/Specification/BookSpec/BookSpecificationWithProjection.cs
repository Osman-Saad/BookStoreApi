using BookStore.Core.Models;
using BookStore.Core.Specification.Projections;

namespace BookStore.Core.Specification.BookSpecification
{
    public class BookSpecificationWithProjection : BaseSpecificationWithProjection<Book, BookProjection>
    {

        public BookSpecificationWithProjection() : base()
        {
            Projection = book => new BookProjection
            {
                Name = book.Name,
                Id = book.Id,
                Category = book.Category.Name,
                Authors = book.Authors.Select(A => A.Name).ToList(),
                CoverUrl = book.CoverUrl,
                Description = book.Description,
                Language = book.Language,
                PageCount = book.PageCount,
                Price = book.Price,
                PublishDate = book.PublishDate,
                Publisher = book.Publisher.Name,
                Quantity = book.Quantity
            };
        }
        public BookSpecificationWithProjection(int id) : base(B => B.Id == id)
        {
            Projection = book => new BookProjection
            {
                Name = book.Name,
                Id = book.Id,
                Category = book.Category.Name,
                Authors = book.Authors.Select(A => A.Name).ToList(),
                CoverUrl = book.CoverUrl,
                Description = book.Description,
                Language = book.Language,
                PageCount = book.PageCount,
                Price = book.Price,
                PublishDate = book.PublishDate,
                Publisher = book.Publisher.Name,
                Quantity = book.Quantity
            };

        }
    }
}
