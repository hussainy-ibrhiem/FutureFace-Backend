using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Base
{
    public interface IAddRepository<T>
    {
        void CreateAsyn(T entity);
        void CreateListAsyn(List<T> entityList);
    }
}
