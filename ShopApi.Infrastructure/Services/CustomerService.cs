using Microsoft.EntityFrameworkCore;
using ShopApi.Core.Dtos;
using ShopApi.Core.Entities;
using ShopApi.Core.Interfaces;
using ShopApi.Infrastructure.Data;

namespace ShopApi.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ShopDbContext _context;

        public CustomerService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    DateOfBirth = c.DateOfBirth,
                    RegistrationDate = c.RegistrationDate
                })
                .ToListAsync();
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return null;

            return MapToCustomerDto(customer);
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto customerDto)
        {
            var customer = new Customer
            {
                FullName = customerDto.FullName,
                DateOfBirth = customerDto.DateOfBirth,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return MapToCustomerDto(customer);
        }

        public async Task<CustomerDto?> UpdateCustomerAsync(CustomerUpdateDto customerDto)
        {
            var customer = await _context.Customers.FindAsync(customerDto.Id);
            if (customer == null) return null;

            customer.FullName = customerDto.FullName ?? customer.FullName;
            customer.DateOfBirth = customerDto.DateOfBirth ?? customer.DateOfBirth;

            await _context.SaveChangesAsync();
            return MapToCustomerDto(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CustomerDto>> GetBirthdayCelebrantsAsync(DateOnly date)
        {
            return await _context.Customers
                .Where(c => c.DateOfBirth.Month == date.Month && c.DateOfBirth.Day == date.Day)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    DateOfBirth = c.DateOfBirth,
                    RegistrationDate = c.RegistrationDate
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<LastPurchaseDto>> GetRecentBuyersAsync(int days)
        {
            var recentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-days));
            return await _context.Purchases
                .Where(p => p.Date >= recentDate)
                .GroupBy(p => p.Customer)
                .Select(g => new LastPurchaseDto
                {
                    CustomerId = g.Key.Id,
                    FullName = g.Key.FullName,
                    LastPurchaseDate = g.Max(p => p.Date)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryPurchaseDto>> GetPopularCategoriesAsync(int customerId)
        {
            return await _context.PurchaseItems
                .Where(pi => pi.Purchase.CustomerId == customerId)
                .GroupBy(pi => pi.Product.Category)
                .Select(g => new CategoryPurchaseDto
                {
                    Category = g.Key,
                    TotalUnits = g.Sum(pi => pi.Quantity)
                })
                .OrderByDescending(g => g.TotalUnits)
                .ToListAsync();
        }

        private static CustomerDto MapToCustomerDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                DateOfBirth = customer.DateOfBirth,
                RegistrationDate = customer.RegistrationDate
            };
        }
    }
}