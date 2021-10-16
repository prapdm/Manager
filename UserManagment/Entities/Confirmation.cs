using System;


namespace Manager.Entities
{
    public class Confirmation
    {
        public int Id { get; set; }
        public int ConfirmationTypeId { get; set; }
        public bool IsConfirmed { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime ValidTo { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ConfirmationType ConfirmationType { get; set; }

    }
}
