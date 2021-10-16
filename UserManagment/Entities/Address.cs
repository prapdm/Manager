using System;


namespace Manager.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int AddressTypeId { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string LocalNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual AddressType AddressType { get; set; }
    }
}
