 

namespace Manager.Models
{
    public class PriceDto
    {
        public int Id { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellPrice { get; set; }
        public string SKU { get; set; }
    }
}
