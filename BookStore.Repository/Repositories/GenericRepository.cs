using BookStore.Core.IRepositories;
using BookStore.Core.Models;
using BookStore.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly BookStoreDbContext dbContext;

        public GenericRepository(BookStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
