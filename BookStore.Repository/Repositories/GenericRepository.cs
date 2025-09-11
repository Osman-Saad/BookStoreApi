using BookStore.Core.IRepositories;
using BookStore.Core.Models;
using BookStore.Core.Specification;
using BookStore.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly BookStoreDbContext dbContext;

        public GenericRepository(BookStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(T model) =>
              await dbContext.Set<T>().AddAsync(model);

        public void DeleteAsync(T model) =>
             dbContext.Set<T>().Remove(model);



        public async Task<IReadOnlyList<T>> GetAllAsync() =>
            await dbContext.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T> specification) =>
           await ApplySpecification(specification).ToListAsync();

        public async Task<IReadOnlyList<TResult>> GetAllAsync<TResult>(ISpecficationWithProjection<T, TResult> specification) =>
            await ApplySpecificationWithProjection<TResult>(specification).ToListAsync();

        public async Task<T?> GetByIdAsync(int id) =>
             await dbContext.Set<T>().FindAsync(id);

        public async Task<T?> GetByIdAsync(ISpecification<T> specification) =>
             await ApplySpecification(specification).FirstOrDefaultAsync();

        public Task<TResult?> GetByIdAsync<TResult>(ISpecficationWithProjection<T, TResult> specification) =>
            ApplySpecificationWithProjection<TResult>(specification).FirstOrDefaultAsync();

        public async Task<int> GetCountAsync<TResult>(ISpecficationWithProjection<T, TResult> spec) =>
            await ApplySpecificationWithProjection<TResult>(spec).CountAsync();

        public async Task<int> GetCountAsync(ISpecification<T> spec) =>
            await ApplySpecification(spec).CountAsync();

        public void UpdateAsync(T model) =>
             dbContext.Set<T>().Update(model);


        private IQueryable<T> ApplySpecification(ISpecification<T> specification) =>
            SpecificationEveluator<T>.GetQuery(dbContext.Set<T>(), specification);

        private IQueryable<TResult> ApplySpecificationWithProjection<TResult>(
            ISpecficationWithProjection<T, TResult> specification
            )
        {
            return SpecificationEveluator<T>.ApplySpecificationWithProjection(dbContext.Set<T>(), specification);
        }

    }
}
