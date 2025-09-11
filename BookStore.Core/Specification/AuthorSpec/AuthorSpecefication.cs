using BookStore.Core.Models;

namespace BookStore.Core.Specification.AuthorSpec
{
    public class AuthorSpecefication : BaseSpecification<Author>
    {
        public AuthorSpecefication(int id) : base(a => a.Id == id)
        {
            Includes.Add(a => a.Books);
        }
        public AuthorSpecefication()
        {
            Includes.Add(a => a.Books);
        }
    }
}
