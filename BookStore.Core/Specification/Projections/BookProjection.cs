namespace BookStore.Core.Specification.Projections
{
    public class BookProjection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PageCount { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; }
        public string? CoverUrl { get; set; }
        public DateOnly PublishDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }
        public ICollection<string> Authors { get; set; } = new HashSet<string>();
    }


}
