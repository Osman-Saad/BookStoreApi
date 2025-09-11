namespace BookStore.Api.Dtos.Author
{
    public class AuthorDto : AuthorBaseDto
    {
        public int Id { get; set; }
        public ICollection<string> Books { get; set; } = new HashSet<string>();

        public int Age { get; set; }
    }
}
