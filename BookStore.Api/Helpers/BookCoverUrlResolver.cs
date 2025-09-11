using AutoMapper;
using BookStore.Api.Dtos.BooksDto;
using BookStore.Core.Specification.Projections;

namespace BookStore.Api.Helpers
{
    public class BookCoverUrlResolver : IValueResolver<BookProjection, BookDto, string>
    {
        private readonly IConfiguration configuration;

        public BookCoverUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(BookProjection source, BookDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.CoverUrl)) return string.Empty;
            return $"{configuration["ApiBaseUrl"]}/{source.CoverUrl}";
        }
    }
}
