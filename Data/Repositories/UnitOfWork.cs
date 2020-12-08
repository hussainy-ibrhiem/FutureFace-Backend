using Data.Context;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext AppDbContext { get; }
        public UnitOfWork(AppDbContext appContext)
        {
            AppDbContext = appContext;
        }
        public IRepository<Product> Product => new Repository<Product>(AppDbContext);

        public Task<int> Commit()
        {
            return AppDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
        }
    }
}
