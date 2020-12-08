using Data.Context;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class Repository<T> : CrudRepository<T>, IRepository<T> where T : class
    {
        public Repository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
