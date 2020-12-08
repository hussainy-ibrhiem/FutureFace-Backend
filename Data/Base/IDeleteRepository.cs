using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Base
{
    public interface IDeleteRepository<T>
    {
        void Delete(T entity);
        void DeleteList(List<T> entityList);
    }
}
