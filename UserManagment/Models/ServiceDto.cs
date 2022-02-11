using Microsoft.AspNetCore.Http;

namespace Manager.Models
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Slug { get; set; }
        public string Tags { get; set; }
        public string SKU { get; set; }
        public bool IsActive { get; set; }
        public bool Update { get; set; }
        public int? CategoryId { get; set; }
        public int? PriceId { get; set; }

    }
}
