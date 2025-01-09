using Bogus;
using Microsoft.EntityFrameworkCore;
using ShopApi.Core.Entities;

namespace ShopApi.Infrastructure.Data
{
    public class DataSeeder
    {
        private readonly ShopDbContext _context;

        public DataSeeder(ShopDbContext context)
        {
            this._context = context;
        }

        public async Task SeedAsync()
        {
            if (await _context.Customers.AnyAsync() || await _context.Products.AnyAsync() || await _context.Purchases.AnyAsync())
            {
                return;
            }

            var customers = GenerateCustomers(10);
            var products = GenerateProducts(20);
            var purchases = GeneratePurchases(customers, products, 30);

            await _context.Customers.AddRangeAsync(customers);
            await _context.Products.AddRangeAsync(products);
            await _context.Purchases.AddRangeAsync(purchases);
            await _context.SaveChangesAsync();
        }

        private List<Customer> GenerateCustomers(int count)
        {
            var customerFaker = new Faker<Customer>()
                .RuleFor(c => c.FullName, f => f.Name.FullName())
                .RuleFor(c => c.DateOfBirth, f => DateOnly.FromDateTime(f.Date.Past(30, DateTime.Now.AddYears(-18))))
                .RuleFor(c => c.RegistrationDate, f => DateOnly.FromDateTime(f.Date.Past(2)));

            return customerFaker.Generate(count);
        }

        private List<Product> GenerateProducts(int count)
        {
            var productFaker = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
                .RuleFor(p => p.SKU, f => f.Commerce.Ean13())
                .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()));

            return productFaker.Generate(count);
        }

        private List<Purchase> GeneratePurchases(List<Customer> customers, List<Product> products, int count)
        {
            var purchaseFaker = new Faker<Purchase>()
                .RuleFor(p => p.Date, f => DateOnly.FromDateTime(f.Date.Past(1)))
                .RuleFor(p => p.TotalCost, f => decimal.Parse(f.Commerce.Price()))
                .RuleFor(p => p.Customer, f => f.PickRandom(customers))
                .RuleFor(p => p.PurchaseItems, f => GeneratePurchaseItems(products, f.Random.Int(1, 5)));

            return purchaseFaker.Generate(count);
        }

        private List<PurchaseItem> GeneratePurchaseItems(List<Product> products, int count)
        {
            var purchaseItemFaker = new Faker<PurchaseItem>()
                .RuleFor(pi => pi.Product, f => f.PickRandom(products))
                .RuleFor(pi => pi.Quantity, f => f.Random.Int(1, 10));

            return purchaseItemFaker.Generate(count);
        }
    }
}