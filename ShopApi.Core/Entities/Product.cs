﻿namespace ShopApi.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
    }
}