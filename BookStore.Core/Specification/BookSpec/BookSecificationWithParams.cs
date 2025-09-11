using BookStore.Core.Models;
using BookStore.Core.Specification.Projections;

namespace BookStore.Core.Specification.BookSpecification
{
    public class BookSecificationWithParams : BaseSpecificationWithProjection<Book, BookProjection>
    {
        public BookSecificationWithParams(BookSpecificationParams bookSpecParams)
            : base(
                 B =>
                 (string.IsNullOrEmpty(bookSpecParams.Search) || B.Name.ToLower().Contains(bookSpecParams.Search))
                 &&
                 (!bookSpecParams.CategoryId.HasValue || B.CategoryId == bookSpecParams.CategoryId)
                 &&
                 (!bookSpecParams.PublisherId.HasValue || B.PublisherId == bookSpecParams.PublisherId)
                 &&
                 (!bookSpecParams.AuthorId.HasValue || B.Authors.Any(A => A.Id == bookSpecParams.AuthorId))
            )
        {

            ApplyPagination(take: bookSpecParams.PageSize, skip: (bookSpecParams.PageIndex - 1) * bookSpecParams.PageSize);
            if (!string.IsNullOrEmpty(bookSpecParams.OrderBy))
            {
                switch (bookSpecParams.OrderBy)
                {
                    case "priceASC":
                        SetOrderByAsc(B => B.Price);
                        break;
                    case "priceDESC":
                        SetOrderByDesc(B => B.Price);
                        break;
                    case "nameASC":
                        SetOrderByAsc(B => B.Name);
                        break;
                    case "nameDESC":
                        SetOrderByDesc(B => B.Name);
                        break;
                    default:
                        SetOrderByAsc(B => B.Id);
                        break;
                }
            }
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
