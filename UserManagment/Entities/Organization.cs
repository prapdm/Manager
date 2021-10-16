using System;


namespace Manager.Entities
{
    public class Organization
    {
        public int Id { get; set; }
        public int? CompanyTypeId { get; set; }
        public string Name { get; set; }
        public string Represantative { get; set; }
        public int TypeId { get; set; }
        public string VatNumber { get; set; }
        public string CompanyNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual CustomerType CompanyType { get; set; }
    }
}
