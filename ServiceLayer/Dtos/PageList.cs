using System.Collections.Generic;

namespace ServiceLayer.Dtos
{
    public class PageList<T>
    {
        public PageList()
        {
            DataList = new List<T>();
        }
        public List<T> DataList { get; set; }
        public int TotalCount { get; set; }

    }
}
