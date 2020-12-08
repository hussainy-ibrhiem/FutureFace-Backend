using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Commit();
        IRepository<Product> Product { get; }
    }
}
