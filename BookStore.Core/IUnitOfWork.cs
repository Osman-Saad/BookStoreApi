using BookStore.Core.IRepositories;
using BookStore.Core.Models;

namespace BookStore.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> CompleteAsync();
    }
}
