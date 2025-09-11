namespace BookStore.Repository.Data.Seeding
{
    internal class BookAuthorSeed
    {
        public string Name { get; set; }
        public int PageCount { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; }

        public string? CoverUrl { get; set; }
        public DateOnly PublishDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public int? PublisherId { get; set; }
        public int? CategoryId { get; set; }
        public ICollection<int> AuthorsIds { get; set; } = new HashSet<int>();
    }
}
