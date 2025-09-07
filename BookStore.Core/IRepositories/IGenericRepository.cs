using BookStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

    }
}
