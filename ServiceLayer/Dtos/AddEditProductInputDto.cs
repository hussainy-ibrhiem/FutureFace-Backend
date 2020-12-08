using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Dtos
{
    public class AddEditProductInputDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public double Price { get; set; }
    }
}
