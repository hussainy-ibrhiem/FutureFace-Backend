using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Base
{
    public interface IEditRepository<T>
    {
        void Update(T entity);
        void UpdateList(List<T> entityList);
    }
}
