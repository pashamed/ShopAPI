namespace ShopApi.Core.Dtos
{
    public class LastPurchaseDto
    {
        public int CustomerId { get; set; }

        public string FullName { get; set; }

        public DateOnly LastPurchaseDate { get; set; }
    }
}