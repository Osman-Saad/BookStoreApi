namespace BookStore.Core.Specification.BookSpecification
{
    public class BookSpecificationParams
    {

        public int PageIndex { get; set; } = 1;
        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 10 ? pageSize : value; }
        }
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public int? PublisherId { get; set; }
        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
        public string? OrderBy { get; set; }
    }
}
