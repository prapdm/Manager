using System;
using System.Collections.Generic;
using Manager.Entities;

namespace Manager.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public int CostomerTypeID { get; set; }
        public int AddressId { get; set; }
        public int CustomerConfirmationId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual List<Address> Address { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual CustomerType CustomerType { get; set; }
        public virtual List<Confirmation> Confirmation { get; set; }
        public virtual List<CustomerService> CustomerServices { get; set; }
    }
}
