using BookStore.Core.Models;
using BookStore.Core.Specification;

namespace BookStore.Core.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task AddAsync(T model);
        public void UpdateAsync(T model);
        public void DeleteAsync(T model);
        public Task<T?> GetByIdAsync(int id);
        public Task<IReadOnlyList<T>> GetAllAsync();

        public Task<T?> GetByIdAsync(ISpecification<T> specification);
        public Task<TResult?> GetByIdAsync<TResult>(ISpecficationWithProjection<T, TResult> specification);
        public Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T> specification);

        public Task<IReadOnlyList<TResult>> GetAllAsync<TResult>(ISpecficationWithProjection<T, TResult> specification);

        public Task<int> GetCountAsync<TResult>(ISpecficationWithProjection<T, TResult> spec);
        public Task<int> GetCountAsync(ISpecification<T> spec);
    }
}
