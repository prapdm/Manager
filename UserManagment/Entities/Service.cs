using System;

namespace Manager.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }
        public string Tags { get; set; }
        public string SKU { get; set; }
        public bool IsActive { get; set; }
        public int? CategoryId { get; set; }
        public int? PriceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Price Price { get; set; }
        public virtual Category Category { get; set; }
    }
}
