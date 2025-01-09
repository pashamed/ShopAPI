namespace ShopApi.Core.Dtos
{
    public class PurchaseDto
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public decimal TotalCost { get; set; }

        public int CustomerId { get; set; }

        public List<PurchaseItemDto>? PurchaseItems { get; set; }
    }

    public class PurchaseCreateDto
    {
        public int CustomerId { get; set; }

        public List<PurchaseItemCreateDto>? PurchaseItems { get; set; }
    }

    public class PurchaseUpdateDto
    {
        public int Id { get; set; }

        public DateOnly? Date { get; set; }

        public decimal? TotalCost { get; set; }

        public int CustomerId { get; set; }

        public List<PurchaseItemUpdateDto>? PurchaseItems { get; set; }
    }
}