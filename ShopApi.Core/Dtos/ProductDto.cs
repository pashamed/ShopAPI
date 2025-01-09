namespace ShopApi.Core.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string SKU { get; set; }

        public decimal Price { get; set; }
    }

    public class ProductCreateDto
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public string SKU { get; set; }

        public decimal Price { get; set; }
    }

    public class ProductUpdateDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Category { get; set; }

        public string? SKU { get; set; }

        public decimal? Price { get; set; }
    }
}