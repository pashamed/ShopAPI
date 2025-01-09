namespace ShopApi.Core.Dtos
{
    public class PurchaseItemDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }

    public class PurchaseItemCreateDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }

    public class PurchaseItemUpdateDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}