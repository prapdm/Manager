using System;


namespace Manager.Entities
{
    public class CustomerService
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PeriodId { get; set; }
        public int ServiceGroupId { get; set; }
        public string SKU { get; set; }
        public bool IsActive { get; set; }
        public DateTime ActiveTill { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ServiceGroup ServiceGroup { get; set; }
}
}
