using System;

namespace Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Deleted { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int? SubCategoryId { get; set; }
    }
}
