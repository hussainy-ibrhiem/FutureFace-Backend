using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Dtos.Catgory
{
    public class CreateCategoryInputDto
    {
        public string Name { get;  set; }
        public int? ParentId { get;  set; }
    }
}
