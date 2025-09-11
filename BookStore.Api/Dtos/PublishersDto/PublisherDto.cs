namespace BookStore.Api.Dtos.Publisher
{
    public class PublisherDto : PublisherBaseDto
    {
        public int Id { get; set; }
        public ICollection<string> Books { get; set; } = new HashSet<string>();
    }
}
