using BookStore.Core.Models;

namespace BookStore.Core.Specification.PublisherSpec
{
    public class PublisherSpecefication : BaseSpecification<Publisher>
    {
        public PublisherSpecefication(int id) : base(p => p.Id == id)
        {
            Includes.Add(P => P.Books);
        }
        public PublisherSpecefication()
        {
            Includes.Add(P => P.Books);
        }
    }
}
