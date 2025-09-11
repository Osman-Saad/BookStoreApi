using BookStore.Core;
using BookStore.Core.IRepositories;
using BookStore.Core.Models;
using BookStore.Repository.Data;
using BookStore.Repository.Repositories;
using System.Collections;

namespace BookStore.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookStoreDbContext dbContext;
        private Hashtable repositories;
        public UnitOfWork(BookStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
            repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await dbContext.DisposeAsync();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T).Name;
            if (!repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<T>(dbContext);
                repositories.Add(type, repository);
            }
            return repositories[type] as IGenericRepository<T>;
        }
    }
}
