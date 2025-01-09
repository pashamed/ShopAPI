namespace ShopApi.Core.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public decimal TotalCost { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}