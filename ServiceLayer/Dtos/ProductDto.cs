using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public double Price { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
    }
}
