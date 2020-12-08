using Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface IRepository<T> : IReadRepository<T>, IAddRepository<T>, IEditRepository<T>, IDeleteRepository<T>
    {
    }
}
