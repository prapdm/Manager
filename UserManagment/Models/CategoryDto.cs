using Microsoft.AspNetCore.Http;
using System.Collections.Generic;


namespace Manager.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Image { get; set; }
        public string Icon { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public bool Update { get; set; }
        public virtual ICollection<CategoryDto> Children { get; set; }

    }
}
