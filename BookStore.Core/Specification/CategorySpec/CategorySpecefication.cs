using BookStore.Core.Models;

namespace BookStore.Core.Specification.CategorySpecification
{
    public class CategorySpecefication : BaseSpecification<Category>
    {
        public CategorySpecefication()
        {
            Includes.Add(c => c.Books);
        }
        public CategorySpecefication(int id) : base(c => c.Id == id)
        {
            Includes.Add(c => c.Books);
        }

    }
}
